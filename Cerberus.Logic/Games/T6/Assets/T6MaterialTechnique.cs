using Cerberus.Logic.Extensions;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialTechnique
    {
        private int NamePtr;
        public string Name = string.Empty;
        public ushort Flags;
        public ushort PassCount;
        public T6MaterialPass[] Pass = new T6MaterialPass[1];
        
        public T6MaterialTechnique(BinaryReader br)
        {
            this.NamePtr = br.ReadInt32();

            this.Flags = br.ReadUInt16();
            this.PassCount = br.ReadUInt16();

            this.Pass = new T6MaterialPass[this.PassCount];
            for (int i = 0; i < this.PassCount; i++)
            {
                this.Pass[i] = new T6MaterialPass(br);
            }

            this.Name = br.ReadNativeString();
        }
    }
}
