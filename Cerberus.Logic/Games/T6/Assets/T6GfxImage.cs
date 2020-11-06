using Cerberus.Logic.Extensions;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6GfxImage
    {
        public T6GfxTexture? Texture;
        public byte MapType;
        public byte Semantic;
        public byte Category;
        public byte DelayLoadPixels;
        public short Platform;
        public byte NoPicmip;
        public byte Track;
        public long CardMemory;
        public short Width;
        public short Height;
        public short Depth;
        public byte LevelCount;
        public byte Streaming;
        public int BaseSize;
        public int Pixels;
        public T6GfxStreamedPartInfo StreamedParts;
        public byte StreamedPartCount;
        public int LoadedSize;
        public byte SkippedMipLevels;
        public string Name;
        public int Hash;

        // Size Of 0x50 (80)
        public T6GfxImage(BinaryReader br)
        {
            int texturePtr = br.ReadInt32();
            this.MapType = br.ReadByte();
            this.Semantic = br.ReadByte();
            this.Category = br.ReadByte();
            this.DelayLoadPixels = br.ReadByte();
            this.Platform = br.ReadInt16();
            this.NoPicmip = br.ReadByte();
            this.Track = br.ReadByte();
            this.CardMemory = br.ReadInt64();
            this.Width = br.ReadInt16();
            this.Height = br.ReadInt16();
            this.Depth = br.ReadInt16();
            this.LevelCount = br.ReadByte();
            this.Streaming = br.ReadByte();
            this.BaseSize = br.ReadInt32();
            this.Pixels = br.ReadInt32();
            this.StreamedParts = new T6GfxStreamedPartInfo(br);
            this.StreamedPartCount = br.ReadByte();

            // Padding
            br.ReadBytes(3);
            
            this.LoadedSize = br.ReadInt32();
            this.SkippedMipLevels = br.ReadByte();

            // Padding

            br.ReadBytes(3);

            int namePtr = br.ReadInt32();
            this.Hash = br.ReadInt32();

            this.Name = br.ReadNativeString();

            if (texturePtr == -1)
            {
                this.Texture = new T6GfxTexture(br);
            }
        }
    }
}