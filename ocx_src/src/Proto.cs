// #define DUMP_BYTES

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace FiscalProto
{
    /// <summary>
    /// Implementa el protocolo de comunicación del Genesis
    /// </summary>
    class Proto : IDisposable
    {
        // Caracteres de control usados por el protocolo
        const int STX = 0x02;
        const int ETX = 0x03;
        const int ACK = 0x06;
        const int NAK = 0x15;
        const int DC2 = 0x12;
        const int DC4 = 0x14;
        const int ESC = 0x1b;
        const int EOF = -1;
        const int SEP = 0x1C;

        VirtualPort port = null;
        bool disposed = false;
        byte packetSequence;
        string remote;
        DateTime downDetect;
        Queue<int> inputQ;
        object locker = new object();
        List<byte> rxBytes = null;

        public Action<bool> IsAlive { get; set; }
        public Action<bool> LowPaper { get; set; }
        public Action<string> OutOfFrame { get; set; }
        public VirtualPort Port { get { return port; } }

        public bool DetectDown { get; set; }

        public bool NAKReceived { get; private set; }
        public int ErrorCode { get; private set; }
        public int PrinterStatus { get; private set; }
        public int FiscalStatus { get; private set; }
        public bool WError { get { return NAKReceived || (ErrorCode != 0); }}
        public string[] Answer { get; private set; }

        public Proto(string remote)
        {
            this.port = null;
            this.remote = remote;
            inputQ = new Queue<int>();
        }

        public Proto(VirtualPort port)
        {
            this.port = port;
            this.remote = null;
            inputQ = new Queue<int>();
        }

        ~Proto() { Dispose(); }

        public void Dispose()
        {
            if (disposed) return;
            if(port != null) port.Dispose();
            port = null;
            GC.SuppressFinalize(this);
        }

        [Conditional("DUMP_BYTES")]
        void DumpPacket(string hdr, IEnumerable<byte> pkt)
        {
            var sb = new StringBuilder();
            var sbText = new StringBuilder();
            sb.Append("  -- ");
            sb.Append(hdr);
            sbText.Append("         ");
            foreach (var b in pkt)
            {
                sb.Append(string.Format(" {0}", b.ToString("X").PadLeft(2, '0')));
                sbText.Append((b >= (byte)' ' || b <= (byte)'z') ?
                    " " + ((char)b).ToString() + " " :
                    "---");
            }

            using (var sw = File.AppendText("log.txt"))
            {
                sw.WriteLine(sb.ToString());
                sw.WriteLine(sbText.ToString());
            }
        }

        public void Lock() { }
        public void Unlock() { }

        // Espera un byte
        private int ReadByte(VirtualPort port)
        {
            downDetect = DateTime.Now;
            while (inputQ.Count <= 0)
            {
                lock (locker)
                {
                    if (!port.IsOpen) return EOF;

                    while (port.BytesToRead)
                    {
                        int ch = port.ReadByte();
                        inputQ.Enqueue(ch);
                        if (downDetect != DateTime.MinValue)
                            downDetect = DateTime.Now;
                    }
                }

                if (DetectDown && (DateTime.Now - downDetect).TotalMilliseconds > 2000)
                    return EOF;

                if(IsAlive != null) IsAlive(true);
            }

            int res = inputQ.Dequeue() & 0xff;
            if(rxBytes != null) rxBytes.Add((byte)res);
            return res;
        }

        // Envia un packete binario
        void TxPacket(VirtualPort port, byte[] data)
        {
            var pkt = new List<byte>();
            int chksum = 0;
            pkt.Add(STX); chksum += STX;
            foreach (var b in data)
            {
                if (b == 0x2 || b == 0x3 || b == 0x1a || b == 0x1b || b == 0x1d || b == 0x1e || b == 0x1f)
                {
                    pkt.Add(ESC); chksum += ESC;
                }
                pkt.Add(b); chksum += b;
            }
            pkt.Add(ETX); chksum += ETX;
            var chksumStr = (chksum & 0xffff).ToString("X").PadLeft(4, '0');
            foreach (var ch in chksumStr)
                pkt.Add((byte)ch);

            // Dump packet
            DumpPacket(">> ", pkt);

            // IMPORTANTE
            // El Genesis no puede manejar mas de 1200 bytes (mas o menos) a la vez (El buffer es enviado al handler y,
            // si mas bytes se envían mientras los anteriores están siendo procesados, estos nuevos bytes se pierden).
            //
            // Para permitir manejar big buffers del lado del impresor, se envían los comandos en batchs de 1000 bytes.
            //
            // Entre cada batch, consumimos eventos de windows y introduzco una pequeña demora (pero solo si hay mas
            // bytes para enviar).
            int frameSz = 1000;
            var aPkt = pkt.ToArray();
            int txPtr = 0;
            while (true)
            {
                var txCount = aPkt.Length - txPtr;
                if (txCount <= 0) break;
                if (txCount > frameSz)
                    txCount = frameSz;

                port.Write(aPkt, txPtr, txCount);
                if (txCount < frameSz)
                {
                    if (IsAlive != null) IsAlive(true);
                    break;
                }

                txPtr += txCount;
                do
                {
                    Thread.Sleep(200);
                } while (port.BytesToWrite);

                if (IsAlive != null) IsAlive(true);
            }
        }


        /// <summary>
        /// Recibe un comando y lo desenpaqueta
        /// </summary>
        /// 
        /// <param name="port">El puerto por el que se reciben los bytes.</param>
        /// 
        /// <returns>Los bytes recibidos, desenpaquetados y validados.</returns>
        private byte[] RxPacket(VirtualPort port)
        {
            rxBytes = new List<byte>();
            NAKReceived = false;
            var pkt = new List<byte>();
            while (true)
            {
                pkt.Clear();
                while (true)
                {
                    var b = ReadByte(port);
                    if (b == EOF) return null;
                    if (b == NAK)
                    {
                        NAKReceived = true;
                        return null;
                    }
                    if (b == STX) break;
                    if (b == ACK || b == DC2 || b == DC4)
                    {
                        if (LowPaper != null && (b == DC2 || b == DC4))
                            LowPaper(b == DC2);
                        continue;
                    }
                    if(OutOfFrame != null)
                        OutOfFrame(((char)b).ToString());
                }

                int chksum = STX;
                while (true)
                {
                    var b = ReadByte(port);
                    if (b == EOF) return null;
                    chksum += b;
                    bool escaped = false;
                    if (b == ESC)
                    {
                        escaped = true;
                        b = ReadByte(port);
                        if (b == EOF)
                            return null;
                        chksum += b;
                    }

                    if (!escaped && b == ETX) break;
                    pkt.Add((byte)b);
                }

                string crc = "";
                for (int icrc = 0; icrc < 4; icrc++)
                {
                    int b = ReadByte(port);
                    if (b == EOF) return null;
                    crc += ((char)b).ToString();
                }
                if (crc == (chksum & 0xffff).ToString("X").PadLeft(4, '0'))
                {
                    if (pkt.Count != 1 || pkt[0] != 0x80)
                    {
                        port.Write(new byte[] { ACK }, 0, 1);
                        break;
                    }
                }
                else
                    port.Write(new byte[] { NAK }, 0, 1);
            }

            DumpPacket("<< ", rxBytes);
            rxBytes = null;
            return pkt.ToArray();
        }

        /// <summary>
        /// Intercambia un comando
        /// </summary>
        /// 
        /// <param name="cmdBin">El comando a enviar al genesis</param>
        /// <returns>The output object (same as <b>cmd.OutputObject</b>)</returns>
        public byte[] ExchangePacketBinLowLevel(byte[] cmdBin)
        {
            byte[] rcvBin = null;

            if (port == null || !port.IsOpen)
            {
                if (remote == null)
                    port.Close();
                else
                {
                    if (port != null) port.Dispose();
                    port = Remote.CreatePort(remote);
                }
                port.Open();
                if (port == null || !port.IsOpen)
                    return null;
            }

            // Patch the sequence
            if (packetSequence <= 0x80 || packetSequence >= 0xff)
                packetSequence = (byte)new Random().Next(0x81, 0xff);
            cmdBin[0] = packetSequence++;
            if (packetSequence <= 0x80 || packetSequence >= 0xff)
                packetSequence = 0x81;

            // Ensure than nothing is in the wire
            while (port.BytesToRead || port.BytesToWrite)
            {
                lock (locker)
                {
                    var sb = new StringBuilder();
                    while (port.BytesToRead)
                    {
                        int ch = port.ReadByte();
                        if (ch == EOF)
                            break;

                        sb.Append((char)ch);
                    }
                    if (sb.Length > 0 && OutOfFrame != null)
                        OutOfFrame(sb.ToString());

                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();
                    inputQ.Clear();
                }

                if (IsAlive != null) IsAlive(true);
                Thread.Sleep(50);
            }

            // Send the command...
            TxPacket(port, cmdBin);

            // Wait an answser and match the packet id
            int retryCount = 5;
            do
            {
                rcvBin = RxPacket(port);
                if (rcvBin == null && NAKReceived)
                {
                    TxPacket(port, cmdBin);
                    continue;
                }

                if (rcvBin == null)
                    break;

            } while ((rcvBin == null && --retryCount > 0) || (rcvBin != null) && rcvBin[0] != cmdBin[0]);

            // Flush (could exists an ACK in the wire)
            while (port.BytesToRead || port.BytesToWrite)
            {
                lock (locker)
                {
                    var sb = new StringBuilder();
                    while (port.BytesToRead)
                    {
                        int ch = port.ReadByte();
                        sb.Append((char)ch);
                    }
                    if (sb.Length > 0 && OutOfFrame != null)
                        OutOfFrame(sb.ToString());
                }

                if (IsAlive != null) IsAlive(true);
            }

            return rcvBin;
        }

        public byte[] ExchangePacketBin(byte[] cmdBin)
        {
            var result = ExchangePacketBinLowLevel(cmdBin);
            if (result == null) return null;
            for (var v = result; (v[2] & 2) != 0;)
            {
                if(LowPaper != null) LowPaper.Invoke(true);
                v = ExchangePacketBinLowLevel(cmdBin);
                Thread.Sleep(100);
            }

            if(LowPaper != null) LowPaper.Invoke(false);
            return result;
        }

        /// <summary>
        /// Exchange a command defined as a sequence of bytes, byte represend as an string.
        /// </summary>
        /// 
        /// <param name="cmd">Command's text</param>
        public bool Exchange(string cmd)
        {
            ErrorCode = 0;
            List<byte> bin = new List<byte>();
            var parts = cmd.Split('|');
            if (parts.Length < 2)
                throw new Exception(string.Format("Comando invalido: '{0}'", cmd));

            bin.Add(0xaa);

            int code = Convert.ToInt32(parts[0], 16);
            bin.Add((byte)(code / 0x100));
            bin.Add((byte)(code & 0xff));

            bin.Add(SEP);
            int exten = Convert.ToInt32(parts[1], 16);
            bin.Add((byte)(exten / 0x100));
            bin.Add((byte)(exten & 0xff));

            for (int i = 2; i < parts.Length; i++)
            {
                bin.Add(SEP);
                foreach (var ch in parts[i])
                    bin.Add((byte)ch);
            }

            Answer = null;
            var rcv = ExchangePacketBin(bin.ToArray());
            if (rcv == null || NAKReceived)
                return false;

            if (rcv.Length >= 10)
            {
                PrinterStatus = rcv[1] * 0x100 + rcv[2];
                FiscalStatus = rcv[4] * 0x100 + rcv[5];
                ErrorCode = rcv[8] * 0x100 + rcv[9];
                if (ErrorCode != 0)
                    return false;
            }

            var sb = new StringBuilder();
            for (int i = 12; i < rcv.Length; i++)
                sb.Append((char)rcv[i]);
            Answer = sb.ToString().Split((char)SEP);
            return true;
        }
    }
}
