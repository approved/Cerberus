using System.IO;

namespace Cerberus.Logic.Games.T4
{
    public class WorldAtWar
    {
        //TODO: Figure out why this doesn't properly deflate
        public static void Decompress(BinaryReader br, BinaryWriter bw)
        {
            bw.Write(Utility.Deflate(br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position))).ToArray());
        }
    }
}
