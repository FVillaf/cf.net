using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FiscalProto
{
    class RNetworkPort : VirtualPort
    {
        TcpClient tcp;
        bool disposed = false;
        bool connected = false;
        Queue<byte> rdata = new Queue<byte>();

        void PollConnection()
        {
            if (tcp == null) return;
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

        IPAddress useIP;

        public RNetworkPort(IPAddress ip)
        {
            useIP = ip;
        }

        ~RNetworkPort()
        {
            Dispose();
        }
    }
}
