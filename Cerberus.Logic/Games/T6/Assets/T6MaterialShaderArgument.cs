using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialShaderArgument
    {
        public ushort Type;
        public T6MaterialArgumentLocation Location;
        public ushort Size;
        public ushort Buffer;
        public T6MaterialArgumentDef ArgDef;

        public T6MaterialShaderArgument(BinaryReader br)
        {
            this.Type = br.ReadUInt16();
            this.Location = new T6MaterialArgumentLocation(br);
            this.Size = br.ReadUInt16();
            this.Buffer = br.ReadUInt16();
            this.ArgDef = new T6MaterialArgumentDef(br);
        }
    }
}