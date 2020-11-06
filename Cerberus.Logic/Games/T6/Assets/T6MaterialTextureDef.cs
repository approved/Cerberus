using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialTextureDef
    {
        public int NameHash;
        public byte NameStart;
        public byte NameEnd;
        public byte SamplerState;
        public byte Semantic;
        public bool IsMatureContent;
        public int GfxImagePtr;
        public T6GfxImage? GfxImage;

        public T6MaterialTextureDef(BinaryReader br)
        {
            this.NameHash = br.ReadInt32();
            this.NameStart = br.ReadByte();
            this.NameEnd = br.ReadByte();
            this.SamplerState = br.ReadByte();
            this.Semantic = br.ReadByte();
            this.IsMatureContent = br.ReadByte() != 0;

            // Padding
            br.ReadBytes(3);

            this.GfxImagePtr = br.ReadInt32();
        }
    }
}
