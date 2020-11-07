using Cerberus.Logic.Extensions;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6RawFile
    {
        public int NamePointer;
        public int DataPointer;
        public int Size;
        public string Name;
        public byte[] Data;

        public T6RawFile(BinaryReader br)
        {
            this.NamePointer = br.ReadInt32();
            this.Size = br.ReadInt32();
            this.DataPointer = br.ReadInt32();

            if (this.NamePointer != -1)
            {
                throw new InvalidDataException("XAsset name pointer entries are not supported");
            }

            this.Name = br.ReadNativeString(0, SeekOrigin.Current, 128);

            if (this.DataPointer != -1)
            {
                throw new InvalidDataException("XAsset data pointer entries are not supported");
            }

            this.Data = br.ReadBytes(this.Size + 1);
        }
    }
}
