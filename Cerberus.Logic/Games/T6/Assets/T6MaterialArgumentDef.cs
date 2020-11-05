using System;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialArgumentDef
    {
        private byte[] _data = Array.Empty<byte>();
        private float GetLiteralConst() => BitConverter.ToSingle(this._data);
        public T6MaterialArgumentCodeConst GetCodeConst() => new T6MaterialArgumentCodeConst()
        {
            Index = BitConverter.ToUInt16(this._data),
            FirstRow = this._data[2],
            RowCount = this._data[3]
        };

        public uint GetCodeSampler() => BitConverter.ToUInt32(this._data);
        public uint NameHash => BitConverter.ToUInt32(this._data);

        public T6MaterialArgumentDef(BinaryReader br)
        {
            this._data = br.ReadBytes(4);
        }
    }
}