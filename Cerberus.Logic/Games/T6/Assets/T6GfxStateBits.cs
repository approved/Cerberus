using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6GfxStateBits
    {
        public int[] LoadBits = new int[2];
        public int BlendState;
        public int DepthStencilState;
        public int RaserizerState;

        public T6GfxStateBits(BinaryReader br)
        {
            this.LoadBits[0] = br.ReadInt32();
            this.LoadBits[1] = br.ReadInt32();
            this.BlendState = br.ReadInt32();
            this.DepthStencilState = br.ReadInt32();
            this.RaserizerState = br.ReadInt32();
        }
    }
}
