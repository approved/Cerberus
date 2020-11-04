using System;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6RawFile
    {
        public long NamePointer;
        public long DataPointer;
        public long Size;
        public string Name = string.Empty;
        private byte[] _data = Array.Empty<byte>();

        public byte[] GetData() => this._data;
        public void SetData(byte[] data)
        {
            if (data.Length != Size + 1)
            {
                throw new ArgumentOutOfRangeException("Input data length did not match expected size");
            }

            this._data = data;
        }
    }
}
