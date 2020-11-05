using Cerberus.Logic.Extensions;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialVertexShader
    {
        private int _namePtr;
        public string Name;
        public T6MaterialShaderProgram Program;

        public T6MaterialVertexShader(BinaryReader br)
        {
            this._namePtr = br.ReadInt32();

            br.ReadBytes(12);

            this.Name = br.PeekNativeString();

            br.BaseStream.Seek(-12, SeekOrigin.Current);

            this.Program = new T6MaterialShaderProgram(br);
        }
    }
}