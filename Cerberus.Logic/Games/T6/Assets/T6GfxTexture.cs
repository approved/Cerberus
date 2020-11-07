using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6GfxTexture
    {
        public byte LevelCount;
        public byte Flags;
        public int Format;
        public int ResourceSize;
        public byte[] Data;

        public T6GfxTexture(BinaryReader br)
        {
            this.LevelCount = br.ReadByte();
            this.Flags = br.ReadByte();

            // Padding
            br.ReadBytes(2);

            this.Format = br.ReadInt32();
            this.ResourceSize = br.ReadInt32();

            this.Data = br.ReadBytes(ResourceSize);
        }
    }
}
