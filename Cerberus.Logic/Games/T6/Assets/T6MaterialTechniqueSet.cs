using Cerberus.Logic.Extensions;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialTechniqueSet
    {
        private const int TechniqueSetCount = 36;

        private int NamePtr;
        public string Name;
        public byte WorldVertexFormat;
        public int[] TechniquePtrList = new int[TechniqueSetCount];
        public T6MaterialTechnique[] Techniques = new T6MaterialTechnique[TechniqueSetCount];

        private T6MaterialTechniqueSet() { }

        public T6MaterialTechniqueSet(byte[] data) : this(new BinaryReader(new MemoryStream(data))) { }

        public T6MaterialTechniqueSet(BinaryReader br)
        {
            this.NamePtr = br.ReadInt32();
            this.WorldVertexFormat = br.ReadByte();

            // Padding
            br.ReadBytes(3);

            for (int i = 0; i < TechniqueSetCount; i++)
            {
                this.TechniquePtrList[i] = br.ReadInt32();
            }

            if (this.NamePtr != -1)
            {
                throw new InvalidDataException("XAsset name pointer entries are not supported");
            }

            this.Name = br.ReadNativeString();

            for (int i = 0; i < TechniqueSetCount; i++)
            {
                if (this.TechniquePtrList[i] == 0)
                {
                    continue;
                }

                if (this.TechniquePtrList[i] != -1)
                {
                    // TODO: Implement Offset To Pointer conversion
                    continue;
                }

                this.Techniques [i] = new T6MaterialTechnique(br);
            }
        }

        public static T6MaterialTechniqueSet LoadSet(BinaryReader br)
        {
            byte[] buffer = br.ReadBytes(152);

            T6MaterialTechniqueSet set = new T6MaterialTechniqueSet();
            set.Name = br.ReadNativeString();

            using (BinaryReader setReader = new BinaryReader(new MemoryStream(buffer)))
            {
                set.NamePtr = setReader.ReadInt32();
                set.WorldVertexFormat = setReader.ReadByte();

                // Padding
                setReader.ReadBytes(3);

                for (int i = 0; i < TechniqueSetCount; i++)
                {
                    set.TechniquePtrList[i] = setReader.ReadInt32();
                }
            }

            for (int i = 0; i < TechniqueSetCount; i++)
            {
                if (set.TechniquePtrList[i] != -1)
                {
                    continue;
                }

                set.Techniques[i] = new T6MaterialTechnique(br);
            }

            return set;
        }
    }
}
