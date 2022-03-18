using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Text;

namespace FiscalProto
{
    class RSerialPort : VirtualPort
    {
        SerialPort port;
        bool disposed = false;

        public override bool IsOpen { get { return (port != null)? port.IsOpen : false; } }
        public override bool BytesToWrite { get { return ((port != null)? port.BytesToWrite: 0) > 0; } }
        public override bool BytesToRead { get { return ((port != null)? port.BytesToRead : 0) > 0; } }
        public override void Write(byte[] buff, int index, int count) { if(port != null) port.Write(buff, index, count); }
        public override int ReadByte() { return (port != null)? port.ReadByte() : 0; }
        public override void DiscardInBuffer() { if(port != null) port.DiscardInBuffer(); }
        public override void DiscardOutBuffer() { if(port != null) port.DiscardOutBuffer(); }
        public override void SetTimeout(int timeout) { /*port.ReadTimeout = port.WriteTimeout = timeout; */}

        public override void Open()
        {
            if (port != null)
            {
                try
                {
                    port.Open();
                    port.RtsEnable = true;
                    port.DtrEnable = true;
                    port.Handshake = Handshake.None;
                    port.ReceivedBytesThreshold = 1;
                    port.ReadTimeout = port.WriteTimeout = SerialPort.InfiniteTimeout;
                }
                catch { }
            }
        }

        public override void Close()
        {
            if (IsOpen)
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

        public RSerialPort(string portName, int speed, Parity parity, int dataBits, StopBits stopBits)
        {
            port = new SerialPort(portName, speed, parity, dataBits, stopBits);
        }

        RSerialPort()
        {
            Dispose();
        }
    }
}
