using Cerberus.Logic.Extensions;
using System;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6Material
    {
        public T6MaterialInfo Info;
        public byte[] StateBitsEntry;
        public byte TextureCount;
        public byte ConstantCount;
        public byte StateBitsCount;
        public byte StateFlags;
        public byte CameraRegion;
        public byte ProbeMipBits;
        public int TechniqueSetPtr;
        public T6MaterialTechniqueSet TechniqueSet;
        public int TextureTablePtr;
        public T6MaterialTextureDef[] TextureTable;
        public int ConstantTablePtr;
        public T6MaterialConstantDef[] ConstantTable;
        public int StateBitsTablePtr;
        public T6GfxStateBits[] StateBitsTable;
        public int ThermalMaterialPtr;
        public T6Material ThermalMaterial;

        private T6Material() { }

        public T6Material(byte[] data) : this(new BinaryReader(new MemoryStream(data))) { }

        // Aligned to 8 bytes - Size Of 0x70 (112)
        public T6Material(BinaryReader br)
        {
            this.StateBitsEntry = br.ReadBytes(36);

            this.TextureCount = br.ReadByte();
            this.ConstantCount = br.ReadByte();
            this.StateBitsCount = br.ReadByte();
            this.StateFlags = br.ReadByte();
            this.CameraRegion = br.ReadByte();
            this.ProbeMipBits = br.ReadByte();

            // Padding
            br.ReadBytes(2);

            this.TechniqueSetPtr = br.ReadInt32();
            this.TextureTablePtr = br.ReadInt32();
            this.ConstantTablePtr = br.ReadInt32();
            this.StateBitsTablePtr = br.ReadInt32();
            this.ThermalMaterialPtr = br.ReadInt32();
        }

        public static T6Material LoadMaterial(BinaryReader br)
        {
            byte[] materialData = br.ReadBytes(112);

            T6MaterialInfo materialInfo = new T6MaterialInfo(materialData[..48]);
            T6Material material = new T6Material(materialData[^64..]);

            materialInfo.Name = br.ReadNativeString();
            material.Info = materialInfo;

            if (material.TechniqueSetPtr == -1)
            {
                material.TechniqueSet = new T6MaterialTechniqueSet(br);
            }

            material.TextureTable = new T6MaterialTextureDef[material.TextureCount];
            if (material.TextureTablePtr == -1)
            {
                for (int i = 0; i < material.TextureCount; i++)
                {
                    material.TextureTable[i] = new T6MaterialTextureDef(br);
                    if (material.TextureTable[i].GfxImagePtr == -1)
                    {
                        material.TextureTable[i].GfxImage = new T6GfxImage(br);

                        if (material.TextureTable[i].GfxImage!.NamePtr == -1)
                        {
                            material.TextureTable[i].GfxImage!.Name = br.ReadNativeString();
                        }

                        int imagePtr = material.TextureTable[i].GfxImage!.TexturePtr;
                        if (imagePtr is -1 || imagePtr is -2)
                        {
                            material.TextureTable[i].GfxImage!.Texture = new T6GfxTexture(br);
                        }
                    }
                }
            }

            material.ConstantTable = new T6MaterialConstantDef[material.ConstantCount];
            if (material.ConstantTablePtr == -1)
            {
                for (int i = 0; i < material.ConstantCount; i++)
                {
                    material.ConstantTable[i] = new T6MaterialConstantDef(br);
                }
            }

            material.StateBitsTable = new T6GfxStateBits[material.StateBitsCount];
            if (material.StateBitsTablePtr == -1)
            {
                for (int i = 0; i < material.StateBitsCount; i++)
                {
                    material.StateBitsTable[i] = new T6GfxStateBits(br);
                }
            }

            if (material.ThermalMaterialPtr == -1)
            {
                material.ThermalMaterial = new T6Material(br);
            }

            return material;
        }
    }
}
