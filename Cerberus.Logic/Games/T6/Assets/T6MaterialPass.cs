using System.Collections.Generic;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialPass
    {
        public int VertexDeclarationPtr;
        public T6MaterialVertexDeclaration? VertexDeclaration;
        public int VertexShaderPtr;
        public T6MaterialVertexShader? VertexShader;
        public int PixelShaderPtr;
        public T6MaterialPixelShader? PixelShader;
        public byte PerPrimArgCount;
        public byte PerObjArgCount;
        public byte StableArgCount;
        public byte CustomSamplerFlags;
        public byte PrecompiledIndex;
        public byte MaterialType;
        public int MaterialShaderArgPtr;
        public List<T6MaterialShaderArgument> ShaderArguments = new List<T6MaterialShaderArgument>();

        public T6MaterialPass(BinaryReader br)
        {
            this.VertexDeclarationPtr = br.ReadInt32();
            this.VertexShaderPtr = br.ReadInt32();
            this.PixelShaderPtr = br.ReadInt32();
            this.PerPrimArgCount = br.ReadByte();
            this.PerObjArgCount = br.ReadByte();
            this.StableArgCount = br.ReadByte();
            this.CustomSamplerFlags = br.ReadByte();
            this.PrecompiledIndex = br.ReadByte();
            this.MaterialType = br.ReadByte();

            br.BaseStream.Seek(br.BaseStream.Position % 4, SeekOrigin.Current);

            this.MaterialShaderArgPtr = br.ReadInt32();

            if (this.VertexShaderPtr == -1)
            {
                this.VertexShader = new T6MaterialVertexShader(br);
            }

            if (this.VertexDeclarationPtr == -1)
            {
                this.VertexDeclaration = new T6MaterialVertexDeclaration(br);
            }

            if (this.PixelShaderPtr == -1)
            {
                this.PixelShader = new T6MaterialPixelShader(br);
            }

            if (this.MaterialShaderArgPtr == -1)
            {
                int count = this.PerPrimArgCount + this.PerObjArgCount + this.StableArgCount;
                for (int i = 0; i < count; i++)
                {
                    this.ShaderArguments.Add(new T6MaterialShaderArgument(br));
                }
            }
        }
    }
}