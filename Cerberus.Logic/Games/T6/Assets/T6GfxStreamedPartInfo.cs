using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6GfxStreamedPartInfo
    {
        public int LevelCountAndSize;
        public int Hash;
        public short Width;
        public short Height;
        private int _bfc;
        private int _bf10;
        private int _bf14;

        // Size Of 0x18 (24)
        public T6GfxStreamedPartInfo(BinaryReader br)
        {
            this.LevelCountAndSize = br.ReadInt32();
            this.Hash = br.ReadInt32();
            this.Width = br.ReadInt16();
            this.Height = br.ReadInt16();
            this._bfc = br.ReadInt32();
            this._bf10 = br.ReadInt32();
            this._bf14 = br.ReadInt32();
        }
    }
}