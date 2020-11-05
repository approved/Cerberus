using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialStreamRouting
    {
        public byte Source;
        public byte Destination;

        public T6MaterialStreamRouting(BinaryReader br)
        {
            this.Source = br.ReadByte();
            this.Destination = br.ReadByte();
        }
    }
}