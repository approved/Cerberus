using Cerberus.Logic.Extensions;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialShaderProgram
    {
        public uint Size;
        public int ProgramPtr;
        public byte[] Program;

        public T6MaterialShaderProgram(BinaryReader br)
        {
            // D3D11VertexShader Pointer - Unused in fastfiles?
            br.ReadBytes(4);

            this.ProgramPtr = br.ReadInt32();
            this.Size = br.ReadUInt32();

            br.ReadNativeString();

            this.Program = br.ReadBytes((int)this.Size);
        }
    }
}