using Cerberus.Logic.Extensions;
using System;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6Material
    {
        public T6MaterialInfo Info;
        public byte[] StateBitsEntry = new byte[36];
        public byte TextureCount;
        public byte ConstantCount;
        public byte StateBitsCount;
        public byte StateFlags;
        public byte CameraRegion;
        public byte ProbeMipBits;
        public T6MaterialTechniqueSet TechniqueSet;
        public T6MaterialTextureDef[] TextureTable = Array.Empty<T6MaterialTextureDef>();
        public T6MaterialConstantDef ConstantTable;
        public T6GfxStateBits StateBitsTable;
        public T6Material ThermalMaterial;

        // Aligned to 8 bytes - Size Of 0x70 (112)
        public T6Material(BinaryReader br)
        {
            // 48 Bytes
            this.Info = new T6MaterialInfo(br);

            this.StateBitsEntry = br.ReadBytes(36);

            this.TextureCount = br.ReadByte();
            this.ConstantCount = br.ReadByte();
            this.StateBitsCount = br.ReadByte();
            this.StateFlags = br.ReadByte();
            this.CameraRegion = br.ReadByte();
            this.ProbeMipBits = br.ReadByte();

            // Padding
            br.ReadBytes(2);

            int techniqueSetPtr = br.ReadInt32();
            int textureTablePtr = br.ReadInt32();
            int constantTablePtr = br.ReadInt32();
            int stateBitsTablePtr = br.ReadInt32();
            int thermalMaterialPtr = br.ReadInt32();

            this.Info.Name = br.ReadNativeString();

            if (techniqueSetPtr == -1)
            {
                this.TechniqueSet = new T6MaterialTechniqueSet(br);
            }

            if (textureTablePtr == -1)
            {
                this.TextureTable = new T6MaterialTextureDef[this.TextureCount];

                for (int i = 0; i < this.TextureCount; i++)
                {
                    this.TextureTable[i] = new T6MaterialTextureDef(br);

                    if (this.TextureTable[i].GfxImagePtr == -1)
                    {
                        this.TextureTable[i].GfxImage = new T6GfxImage(br);
                    }
                }
            }


            //this.ConstantTable = new T6MaterialConstantDef(br);
            //this.StateBitsTable = new T6GfxStateBits(br);
            //this.ThermalMaterial = new T6Material(br);
        }

        public static void Load(XAssetList assetList, BinaryReader br)
        {
            assetList.Entries.Add(new T6Material(br));
        }
    }
}
