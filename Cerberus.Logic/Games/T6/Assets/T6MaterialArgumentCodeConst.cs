using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialArgumentCodeConst
    {
        public ushort Index;
        public byte FirstRow;
        public byte RowCount;

        public T6MaterialArgumentCodeConst() { }

        public T6MaterialArgumentCodeConst(BinaryReader br)
        {
            Index = br.ReadUInt16();
            FirstRow = br.ReadByte();
            RowCount = br.ReadByte();
        }
    }
}