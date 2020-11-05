﻿using Cerberus.Logic.Extensions;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialTechniqueSet
    {
        private const int TechniqueSetCount = 36;

        private int NamePtr;
        public string Name = string.Empty;
        public byte WorldVertexFormat;
        public int[] TechniquePtrList = new int[TechniqueSetCount];
        public T6MaterialTechnique[] Techniques = new T6MaterialTechnique[TechniqueSetCount];

        public T6MaterialTechniqueSet(BinaryReader br)
        {
            this.NamePtr = br.ReadInt32();

            if (this.NamePtr != -1)
            {
                throw new InvalidDataException("XAsset name pointer entries are not supported");
            }

            this.Name = br.ReadNativeString();

            // TODO: Properly Align - Reading will fail if string is less than 3 characters + null char
            if (string.IsNullOrEmpty(this.Name))
            {
                br.ReadBytes(3);
            }

            for (int i = 0; i < TechniqueSetCount; i++)
            {
                this.TechniquePtrList[i] = br.ReadInt32();
            }

            for (int i = 0; i < TechniqueSetCount; i++)
            {
                if (this.TechniquePtrList[i] != -1)
                {
                    continue;
                }

                this.Techniques [i] = new T6MaterialTechnique(br);
            }
        }

        public static void Load(XAssetList assetList, BinaryReader br)
        {
            assetList.Entries.Add(new T6MaterialTechniqueSet(br));
        }
    }
}
