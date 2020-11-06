using Cerberus.Logic.Extensions;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialVertexShader
    {
        private int _namePtr;
        public string Name;
        public uint Size;
        public int ProgramPtr;
        public byte[] Program;

        public T6MaterialVertexShader(BinaryReader br)
        {
            this._namePtr = br.ReadInt32();

            // D3D11VertexShader Pointer - Unused in fastfiles?
            br.ReadBytes(4);

            this.ProgramPtr = br.ReadInt32();
            this.Size = br.ReadUInt32();

            this.Name = br.ReadNativeString();

            this.Program = br.ReadBytes((int)this.Size);
        }
    }
}