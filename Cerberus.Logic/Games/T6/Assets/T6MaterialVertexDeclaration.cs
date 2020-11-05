using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialVertexDeclaration
    {
        public byte StreamCount;
        public bool HasOptionalSource;
        public bool IsLoaded;
        public T6MaterialVertexStreamRouting Routing;

        public T6MaterialVertexDeclaration(BinaryReader br)
        {
            this.StreamCount = br.ReadByte();
            this.HasOptionalSource = br.ReadByte() != 0;
            this.IsLoaded = br.ReadByte() != 0;

            br.ReadByte();

            this.Routing = new T6MaterialVertexStreamRouting(br);
        }
    }
}