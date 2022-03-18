using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalProto
{
    abstract class VirtualPort : IDisposable
    {
        public int InfiniteTimeout { get { return -1; } }

        public abstract bool IsOpen { get; }
        public abstract bool BytesToWrite { get; }
        public abstract bool BytesToRead { get; }
        public abstract void Write(byte[] buff, int index, int count);
        public abstract int ReadByte();
        public abstract void DiscardInBuffer();
        public abstract void DiscardOutBuffer();
        public abstract void Dispose();
        public abstract void SetTimeout(int timeout);
        public abstract void Open();
        public abstract void Close();
    }
}

