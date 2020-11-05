using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialArgumentLocation
    {
        private ushort _offset;
        public ushort GetOffset() => this._offset;
        public int GetTextureIndex() => this._offset & 0x00FF;
        public int GetSamplerIndex() => this._offset & 0xFF00;

        public T6MaterialArgumentLocation(BinaryReader br)
        {
            this._offset = br.ReadUInt16();
        }
    }
}