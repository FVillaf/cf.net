using System;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Collections.Generic;

#if !_CONSOLE
using System.Windows.Forms;
#endif

namespace FiscalProto
{
    /// <summary>
    /// Manejador de protocolo que intercambia comandos con un dispositivo.
    /// </summary>
    public class ProtoHandler : IDisposable
    {
        // La descripción del port.
        TargetBag target;

        // La definicion del puerto
        BatchArgs batchArgs;

        // La queue a usar para mantener los bytes recibidos
        Queue<int> inputQ;

        // La cantidad de frames 'procesando...' recibidos
        int processingCount;

        // Flag usado para conocer que se está cancelando.
        bool cancelling;

        // El número de secuencia del paquete.
        byte packetSequence;

        /// <summary>
        /// El puerto usado
        /// </summary>
        VirtualPort usedPort;

        // Caracteres de control
        const int STX = 0x02;
        const int ETX = 0x03;
        const int ACK = 0x06;
        const int NAK = 0x15;
        const int DC2 = 0x12;
        const int DC4 = 0x14;
        const int ESC = 0x1b;
        const int EOF = -1;
        const int SEP = 0x1C;

        // Sincro multithread
        object locker = new object();

        // Campo de ayuda para detectar que se cayó la comunicación
        DateTime downDetect = DateTime.MinValue;

        /// <summary>
        /// El puerto que está usando.
        /// </summary>
        public VirtualPort Port => usedPort;

        /// <summary>
        /// El timeout para las operaciones de lectura
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// El último error devuelto por el protocolo fiscal
        /// </summary>
        public string LastError { get; set; }

        /// <summary>
        /// La última respuesta recibida desde el controlador fiscal.
        /// </summary>
        public string Answer { get; private set; }

        /// <summary>
        /// Indica si verificamos o no la ocurrencia de DC4
        /// </summary>
        public bool VerifyDC4 { get; set; } = false;

        /// <summary>
        /// Método al que enviar los bytes que se reciban fuera del frame fiscal.
        /// </summary>
        public Action<string> RxOutOfFrame;

        /// <summary>
        /// Método que se invocará cada vex que se detecte que cambia la condición de
        /// 'poco-papel'.
        /// </summary>
        public Action<bool> LowPaper;

        /// <summary>
        /// Indica que el protocolo está en modo 'cancelación'
        /// </summary>
        public bool Cancelled
        {
            get { return cancelling; }
        }

        /// <summary>
        /// Indica si se grabará en el log el detalle de los bytes intercambiados con
        /// el controlador fiscal.
        /// </summary>
        public bool KeepLog
        {
            get { return (Log != null); }
            set
            {
                if (value)
                {
                    if (Log == null) Log = new List<string>();
                }
                else
                {
                    Log = null;
                }
            }
        }

        /// <summary>
        /// El descriptor del dispositivo conectado.
        /// </summary>
        public TargetBag Bag
        {
            get { return target; }
        }

        /// <summary>
        /// La lista de mensajes de log generados durante la sesión.
        /// </summary>
        public List<string> Log
        {
            get;
            private set;
        }

        /// <summary>
        /// La cantidad de frames de espera recibidos desde el controlador fiscal.
        /// </summary>
        public int ProcessingWaitCount
        {
            get { return processingCount; }
        }

        /// <summary>
        /// Evento a llamar si el handler debe manejar un token de cancelación.
        /// </summary>
        public Action OnCancelled { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        /// <param name="target">El target con el cuál comunicarse.</param>
        public ProtoHandler(TargetBag target)
        {
            inputQ = new Queue<int>();
            this.target = target;
            this.Timeout = 5000;
            this.usedPort = ConstructPort();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        /// <param name="batchArgs">La configuración recuperada de un archivo.</param>
        public ProtoHandler(BatchArgs batchArgs)
        {
            inputQ = new Queue<int>();
            this.Timeout = 5000;
            this.batchArgs = batchArgs;
            this.usedPort = ConstructPort();
        }

        /// <summary>
        /// Cancelar la operación en curso.
        /// </summary>
        public void Cancel()
        {
            cancelling = true;
        }

        /// <summary>
        /// Resumir (si se puede) la operación cancelada.
        /// </summary>
        public void ResetCancel()
        {
            cancelling = false;
        }

        /// <summary>
        /// Agrega una cadena a la lista de log.
        /// </summary>
        /// 
        /// <param name="key">La operación que generó el log.</param>
        /// <param name="data">Los datos a loguear</param>
        private void AddToLog(string key, IEnumerable<byte> data = null)
        {
            if (Log == null) return;

            var sb = new StringBuilder();
            var sb2 = new StringBuilder();

            sb.Append(key);
            if (data != null)
            {
                sb.Append(": ");
                for (int i = key.Length + 2; i > 0; i--) sb2.Append(' ');

                foreach (var b in data)
                {
                    if (b == 0x1c)
                    {
                        sb.Append(" 1C ");
                        sb2.Append(" |  ");
                    }
                    else
                    {
                        sb.Append(b.ToString("X").PadLeft(2, '0'));
                        if (b >= ' ' && b <= 'z')
                        {
                            sb2.Append((char)b);
                            sb2.Append(' ');
                        }
                        else
                            sb2.Append("..");
                    }
                }
            }
            Log.Add(sb.ToString());
            Log.Add(sb2.ToString());
        }

        /// <summary>
        /// Envía un paquete, envolviéndolo en los caracteres de control
        /// </summary>
        /// 
        /// <param name="data">Los datos a enviar.</param>
        private void TxPacket(byte[] data)
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

            AddToLog("TX", pkt);

            // IMPORTANTE!!
            // El impresor no es capaz de manejar mas de 1200 bytes (mas o menos) de una sola vez.
            // Para manejar el envío de buffers grandes, enviamos el comando en lotes de no más de 1000 bytes
            // a la vez e introducimos una demora entre lote y lote.
            int frameSz = 1000;
            if (!cancelling)
            {
                var aPkt = pkt.ToArray();
                int txPtr = 0;
                while (true)
                {
                    if (cancelling) return;

                    var txCount = aPkt.Length - txPtr;
                    if (txCount <= 0) break;
                    if (txCount > frameSz)
                        txCount = frameSz;

                    usedPort.Write(aPkt, txPtr, txCount);
                    if (txCount < frameSz)
                    {
                        Application.DoEvents();
                        break;
                    }

                    txPtr += txCount;
                    do
                    {
                        Thread.Sleep(200);
                    } while (usedPort.BytesToWrite);

#if !_CONSOLE
                    Application.DoEvents();
#endif
                }
            }
        }
    
        /// <summary>
        /// Indica que se recibió un NAK desde el controlador fiscal, señalando que recibió
        /// datos con un chksum incorrecto.
        /// </summary>
        public bool NAKReceived { get; private set; }

        /// <summary>
        /// Recibe un comando y lo desenpaqueta.
        /// </summary>
        /// 
        /// <returns>Los bytes recibidos, validados y desenpaquetados..</returns>
        private byte[] RxPacket()
        {
            processingCount = 0;
            NAKReceived = false;
            var pkt = new List<byte>();
            while (true)
            {
                pkt.Clear();
                while (true)
                {
                    var b = ReadByte(usedPort);
                    if (cancelling || (b == EOF)) return null;
                    if(b == NAK)
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
                    RxOutOfFrame?.Invoke(((char)b).ToString());
                }

                int chksum = STX;
                while (true)
                {
                    var b = ReadByte(usedPort);
                    if (cancelling || (b == EOF)) return null;
                    chksum += b;
                    bool escaped = false;
                    if (b == ESC)
                    {
                        escaped = true;
                        b = ReadByte(usedPort);
                        if (cancelling || (b == EOF))
                            return null;
                        chksum += b;
                    }

                    if (!escaped && b == ETX) break;
                    pkt.Add((byte)b);
                }

                string crc = "";
                for (int icrc = 0; icrc < 4; icrc++)
                {
                    int b = ReadByte(usedPort);
                    if (cancelling || (b == EOF)) return null;
                    crc += ((char)b).ToString();
                }
                if (crc == (chksum & 0xffff).ToString("X").PadLeft(4, '0'))
                {
                    if (pkt.Count != 1 || pkt[0] != 0x80)
                    {
                        usedPort.Write(new byte[] { ACK }, 0, 1);
                        break;
                    }
                    else
                        processingCount++;
                }
                else
                    usedPort.Write(new byte[] { NAK }, 0, 1);
            }

            AddToLog("RX", pkt);
            return pkt.ToArray();
        }

        /// <summary>
        /// Intercambia un comando con el controlador fiscal.
        /// </summary>
        /// 
        /// <param name="cmd">El comando a enviar</param>
        /// <returns>La respuesta recibida</returns>
        public MOutput ExchangePacket(CMD_Generic cmd)
        {
            LastError = null;
            var cmdBin = ((MInput)cmd.InputObject).GetCommand();
            var mo = (MOutput)cmd.OutputObject;
            if (!cancelling)
            {
                var rcvBin = ExchangePacketBin(cmdBin);
                if (rcvBin == null)
                    cancelling = true;

                if (!cancelling)
                {
                    mo.SetFromCommand(rcvBin);
                    if (mo.ErrorCodeInt != 0)
                    {
                        LastError = $"Error {mo.ErrorCodeInt.ToString("X").PadLeft(4, '0')}-'{mo.ErrorCode}'\nComando: {cmd}";
                        return null;
                    }
                }
                return mo;
            }
            return cancelling? null: mo;
        }

        /// <summary>
        /// Intercambia un comando recibido como string.
        /// </summary>
        /// 
        /// <param name="cmd">El comando a enviar</param>
        /// <returns><b>true</b> si todo salió bien.</returns>
        public bool ExchangePacket(string cmd)
        {
            int errorCode;

            while (true)
            {
                LastError = null;
                bool res = ExecuteExchangePacket(cmd, out errorCode);
                if (res) return true;
                if (errorCode != 0x0304)
                {
                    LastError = $"Error {errorCode.ToString("X").PadLeft(4, '0')}-'{ErrorCodes.Get(errorCode)}'\nComando: {cmd}";
                    return false;
                }
            }
        }

        public bool Ping()
        {
            try
            {
                return ExchangePacket("0001|0000");
            }
            catch { return false; }
        }


        /// <summary>
        /// Exchange a command defined as a sequence of bytes, byte represend as an string.
        /// </summary>
        /// 
        /// <param name="cmd">Command's text</param>
        bool ExecuteExchangePacket(string cmd, out int errorCode)
        {
            errorCode = 0;
            List<byte> bin = new List<byte>();
            var parts = cmd.Split('|');
            if (parts.Length < 2)
                throw new Exception($"Comando invalido: '{cmd}'");

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
            if (rcv == null)
                return false;

            if (rcv.Length >= 10)
            {
                //printerStatus = rcv[1] * 0x100 + rcv[2];
                //var fiscalStatus = rcv[4] * 0x100 + rcv[5];
                errorCode = rcv[8] * 0x100 + rcv[9];
                if (errorCode != 0)
                    return false;
            }

            var sb = new StringBuilder();
            for (int i = 12; i < rcv.Length; i++)
                sb.Append((char)rcv[i]);
            Answer = sb.ToString();
            return true;
        }

        /// <summary>
        /// Construye el puerto por el cuál se comunicará.
        /// </summary>
        /// 
        /// <returns></returns>
        VirtualPort ConstructPort()
        {
            if (usedPort == null)
            {
                if (batchArgs != null)
                    usedPort = VirtualPort.FromBatchArgs(batchArgs);
                else
                    usedPort = VirtualPort.FromTargetBag(target);

                usedPort.SetTimeout(Timeout);
            }
            return usedPort;
        }

        /// <summary>
        /// Intercambia datos en binario
        /// </summary>
        /// 
        /// <param name="cmdBin"></param>
        /// <returns></returns>
        public byte[] ExchangePacketBin(byte[] cmdBin)
        {
            var result = ExchangePacketBinLowLevel(cmdBin);
            if (result == null) return null;
            for (var v = result; (v[2] & 2) != 0;)
            {
                LowPaper?.Invoke(true);
                v = ExchangePacketBinLowLevel(new byte[] { 169, 0, 1, 28, 0, 0 });
                Thread.Sleep(100);
            }
            LowPaper?.Invoke(false);
            return result;
        }

        /// <summary>
        /// Indica si el controlador fiscal está o no disponible para recibir comandos.
        /// </summary>
        public bool Available => true;

        /// <summary>
        /// Realiza el intercambio de comandos basandose en bytes
        /// </summary>
        /// 
        /// <param name="cmdBin">The command to send to printer</param>
        /// <returns>The output object (same as <b>cmd.OutputObject</b>)</returns>
        public byte[] ExchangePacketBinLowLevel(byte[] cmdBin)
        {
            byte[] rcvBin = null;

            usedPort = ConstructPort();
            usedPort.Open();
            if (usedPort == null || !usedPort.IsOpen)
                return null;

            cancelling = false;

            // Patch the sequence
            if (packetSequence <= 0x80 || packetSequence >= 0xff)
                packetSequence = (byte)new Random().Next(0x81, 0xff);
            cmdBin[0] = packetSequence++;
            if (packetSequence <= 0x80 || packetSequence >= 0xff)
                packetSequence = 0x81;

            // Ensure than nothing is in the wire
            while (usedPort.BytesToRead || usedPort.BytesToWrite)
            {
                lock (locker)
                {
                    var sb = new StringBuilder();
                    while (usedPort.BytesToRead)
                    {
                        int ch = usedPort.ReadByte();
                        if (cancelling || ch == EOF)
                            break;

                        sb.Append((char)ch);
                    }
                    if (sb.Length > 0 && RxOutOfFrame != null)
                        RxOutOfFrame(sb.ToString());

                    usedPort.DiscardInBuffer();
                    usedPort.DiscardOutBuffer();
                    inputQ.Clear();
                }
#if !_CONSOLE
                    Application.DoEvents();
#endif
                Thread.Sleep(50);
            }

            // Send the command...
            TxPacket(cmdBin);

            // Wait an answser and match the packet id
            do
            {
                if (cancelling) break;
                rcvBin = RxPacket();
                if(rcvBin == null && NAKReceived)
                {
                    TxPacket(cmdBin);
                    continue;
                }

                if (cancelling || rcvBin == null)
                    break;

            } while (rcvBin == null || (rcvBin[0] != cmdBin[0]));

            // Flush (could exists an ACK in the wire)
            while (usedPort.BytesToRead || usedPort.BytesToWrite)
            {
                lock (locker)
                {
                    var sb = new StringBuilder();
                    while (usedPort.BytesToRead)
                    {
                        int ch = usedPort.ReadByte();
                        sb.Append((char)ch);
                    }
                    if (sb.Length > 0 && RxOutOfFrame != null)
                        RxOutOfFrame(sb.ToString());
                }

#if !_CONSOLE
                    Application.DoEvents();
#endif
            }

            usedPort.Close();
            if (cancelling && OnCancelled != null)
            {
                OnCancelled();
                return null;
            }

            return rcvBin;
        }

        /// <summary>
        /// Read a byte, handling cancellation and non blocking.
        /// </summary>
        /// 
        /// <returns>The readen byte, or EOF if cancelling.</returns>
        private int ReadByte(VirtualPort port)
        {
            if (downDetect != DateTime.MinValue)
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

                if (cancelling)
                    return EOF;

                if (VerifyDC4 && (DateTime.Now - downDetect).TotalMilliseconds > 2000)
                    return EOF;

#if !_CONSOLE
                Application.DoEvents();
#endif
            }

            return inputQ.Dequeue() & 0xff;
        }

        bool disposed = false;

        public void Close()
        {
            if (usedPort != null) usedPort.Close();
            //usedPort = null;
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            Close();
            GC.SuppressFinalize(this);
        }

        ~ProtoHandler() { Dispose(); }
    }
}