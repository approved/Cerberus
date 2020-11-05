using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialVertexStreamRouting
    {
        public T6MaterialStreamRouting[] Data = new T6MaterialStreamRouting[16];
        public int[] Declaration = new int[20];

        public T6MaterialVertexStreamRouting(BinaryReader br)
        {
            for (int i = 0; i < this.Data.Length; i++)
            {
                this.Data[i] = new T6MaterialStreamRouting(br);
            }

            for (int i = 0; i < this.Declaration.Length; i++)
            {
                this.Declaration[i] = br.ReadInt32();
            }
        }
    }
}