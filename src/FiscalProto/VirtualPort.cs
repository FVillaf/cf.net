using System;
using System.Threading;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace FiscalProto
{
    /// <summary>
    /// Clase abstracta para describir la interface de un 'puerto virtual' usado para comunicarse
    /// con el controlador fiscal.
    /// </summary>
    public abstract class VirtualPort : IDisposable
    {
        public int InfiniteTimeout {  get { return -1; } }

        /// <summary>
        /// El nombre (y configuración descriptiva) del puerto virtual usado
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Indica si el puerto está o no abierto.
        /// </summary>
        public abstract bool IsOpen { get; }

        /// <summary>
        /// Indica la cuenta de bytes que están pendientes de ser enviados al controlador fiscal.
        /// </summary>
        public abstract bool BytesToWrite { get; }

        /// <summary>
        /// Indica la cuenta de bytes que el controlador fiscal envió pero todavía no se usaron.
        /// </summary>
        public abstract bool BytesToRead { get; }

        /// <summary>
        /// Escribe un buffer de bytes en el puerto virtual
        /// </summary>
        /// 
        /// <param name="buff">El buffer de bytes a enviar</param>
        /// <param name="index">El primer byte del buffer a enviar</param>
        /// <param name="count">La cantidad de bytes a enviar</param>
        public abstract void Write(byte[] buff, int index, int count);

        /// <summary>
        /// Lee un byte desde el puerto serial
        /// </summary>
        /// 
        /// <returns>El byte recibido</returns>
        /// <remarks>
        /// Este método usa lo que se configure por SetTimeout como tiempo de espera antes de 
        /// devolver -1 (que sería la indicación de timeout)
        /// </remarks>
        public abstract int ReadByte();

        /// <summary>
        /// Descarta todos los bytes que se hayan recibido pero no procesados.
        /// </summary>
        public abstract void DiscardInBuffer();

        /// <summary>
        /// Descarta todos los bytes que todavía no se hayan enviado al controlador fiscal.
        /// </summary>
        public abstract void DiscardOutBuffer();

        /// <summary>
        /// Finaliza con el uso de ésta clase
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Indica el tiempo (en milisegundos) durante el cuál espera que entre un byte.
        /// </summary>
        /// 
        /// <param name="timeout">El tiemout en milisegundos</param>
        public abstract void SetTimeout(int timeout);

        /// <summary>
        /// Abre el puerto y lo deja listo para enviar y recibir datos.
        /// </summary>
        public abstract void Open();

        /// <summary>
        /// Cierra el puerto.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Mètodo fábrica, a partir de argumentos guardados en un archivo de configuración
        /// </summary>
        /// 
        /// <param name="args">Los parámetros capturados del archivo.</param>
        /// <returns></returns>
        public static VirtualPort FromBatchArgs(BatchArgs args)
        {
            if (!args.UseSocket)
                return new VirtualSerialPort(args.ComPortName, args.ComSpeed, args.ComParity, args.ComNumBits, args.CompStopBits);
            return new VirtualEthernetPort(args.IP);
        }

        /// <summary>
        /// Método fábrica, a partir del tipo de dispositivo conectado.
        /// </summary>
        /// 
        /// <param name="bag"></param>
        /// <returns></returns>
        public static VirtualPort FromTargetBag(TargetBag bag)
        {
            if (!bag.UseNetwork)
                return new VirtualSerialPort(bag.PortName, (int)bag.Speed, Parity.None, 8, StopBits.One);
            return new VirtualEthernetPort(IPAddress.Parse(bag.IPAddress));
        }
    }

    /// <summary>
    /// Implementación de <see cref="VirtualPort"/> que permite comunicarse con el controlador fiscal usando
    /// un puerto serie.
    /// </summary>
    public class VirtualSerialPort : VirtualPort
    {
        SerialPort port;
        bool disposed = false;

        public override string Name => $"{port.PortName} ({port.BaudRate})";

        public override bool IsOpen { get { return port?.IsOpen ?? false; } }
        public override bool BytesToWrite { get { return (port?.BytesToWrite ?? 0) > 0; } }
        public override bool BytesToRead { get { return (port?.BytesToRead ?? 0)> 0; } }
        public override void Write(byte[] buff, int index, int count) { port?.Write(buff, index, count); }
        public override int ReadByte() { return port?.ReadByte() ?? 0; }
        public override void DiscardInBuffer() { port?.DiscardInBuffer(); }
        public override void DiscardOutBuffer() { port?.DiscardOutBuffer(); }
        public override void SetTimeout(int timeout) { /*port.ReadTimeout = port.WriteTimeout = timeout; */}

        public override void Open()
        {
            if (port != null)
            {
                port.Open();
                port.RtsEnable = true;
                port.DtrEnable = true;
                port.Handshake = Handshake.None;
                port.ReceivedBytesThreshold = 1;
                port.ReadTimeout = port.WriteTimeout = SerialPort.InfiniteTimeout;
            }
        }

        public override void Close()
        {
            if (port.IsOpen)
                port.Close();
        }
        public override void Dispose()
        {
            if (disposed) return;
            disposed = true;
            if (port != null)
            {
                port.Dispose();
                port = null;
            }
            GC.SuppressFinalize(this);
        }

        public VirtualSerialPort(string portName, int speed, Parity parity, int dataBits, StopBits stopBits)
        {
            port = new SerialPort(portName, speed, parity, dataBits, stopBits);
        }

        ~VirtualSerialPort()
        {
            Dispose();
        }
    }

    /// <summary>
    /// Implementación de <see cref="VirtualPort"/> usada para conectarse con el controlador fiscal usando
    /// red ethernet.
    /// </summary>
    public class VirtualEthernetPort : VirtualPort
    {
        IPAddress useIP;
        TcpClient tcp;
        bool disposed = false;
        bool connected = false;
        Queue<byte> rdata = new Queue<byte>();

        public override string Name => $"Ethernet ({useIP})";

        void PollConnection()
        {
            if (tcp.Client.Poll(0, SelectMode.SelectRead))
            {
                try
                {
                    byte[] tb = new byte[100];
                    var rcount = tcp.Client.Receive(tb);
                    if (rcount == 0)
                        connected = false;
                    else
                    {
                        for (int i = 0; i < rcount; i++)
                            rdata.Enqueue(tb[i]);
                    }
                }
                catch { connected = false; }
            }
        }

        public override bool IsOpen
        {
            get
            {
                PollConnection();
                return connected;
            }
        }

        public override bool BytesToWrite { get { return false; } }

        public override bool BytesToRead
        {
            get
            {
                PollConnection();
                return (connected ? (rdata.Count > 0) : false);
            }
        }

        public override void Write(byte[] buff, int index, int count)
        {
            if (count == 0 || !connected)
                return;

            byte[] sb = new byte[count];
            Array.Copy(buff, index, sb, 0, count);
            tcp.Client.Send(sb);
        }

        public override int ReadByte()
        {
            PollConnection();
            if (!connected || rdata.Count <= 0)
                throw new Exception("Se interrumpió la conexión con el impresor");
            return rdata.Dequeue();
        }

        public override void DiscardInBuffer()
        {
            var stream = tcp.GetStream();
            while (stream.DataAvailable)
                stream.ReadByte();
        }

        public override void DiscardOutBuffer()
        {
        }

        public override void Dispose()
        {
            if (disposed) return;
            Close();
            tcp = null;
            GC.SuppressFinalize(this);
        }

        int useTimeout = -1;

        public override void SetTimeout(int timeout)
        {
            useTimeout = timeout;
            if (tcp != null)
            {
                tcp.ReceiveTimeout = tcp.SendTimeout = (useTimeout <= 0) ? 0 : useTimeout;
            }
        }

        public override void Open()
        {
            tcp = new TcpClient();
            tcp.LingerState.Enabled = true;
            tcp.LingerState.LingerTime = 2;
            tcp.NoDelay = true;
            tcp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Debug, true);
            tcp.ReceiveTimeout = tcp.SendTimeout = (useTimeout <= 0) ? 0 : useTimeout;

            var aconn = tcp.BeginConnect(useIP, 5003, null, null);
            connected = aconn.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(200));
            if (connected)
                Thread.Sleep(50);
        }

        public override void Close()
        {
            if (tcp != null && tcp.Connected)
            {
                tcp.Client.Shutdown(SocketShutdown.Both);
                tcp.Close();
                tcp = null;
                Thread.Sleep(50);
            }
        }

        public VirtualEthernetPort(IPAddress ip)
        {
            useIP = ip;
        }

        ~VirtualEthernetPort()
        {
            Dispose();
        }
    }
}
