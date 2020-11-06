using Cerberus.Logic.Extensions;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6MaterialInfo
    {
        public string Name;
        public uint GameFlags;
        // Noted for consumption during reading
        private byte _pad;
        public byte SortKey;
        public byte TextureAtlasRowCount;
        public byte TextureAtlasColumnCount;
        public T6GfxDrawSurface DrawSurface;
        public uint SurfaceTypeBits;
        public uint LayeredSurfaceTypes;
        public ushort HashIndex;
        public int SurfaceFlags;
        public int Contents;

        // Aligned to 8 bytes
        public T6MaterialInfo(BinaryReader br)
        {
            int namePtr = br.ReadInt32();

            GameFlags = br.ReadUInt32();
            
            _pad = br.ReadByte();
            SortKey = br.ReadByte();
            TextureAtlasRowCount = br.ReadByte();
            TextureAtlasColumnCount = br.ReadByte();

            // Padding
            br.ReadBytes(4);

            DrawSurface = new T6GfxDrawSurface(br);
            SurfaceTypeBits = br.ReadUInt32();
            LayeredSurfaceTypes = br.ReadUInt32();
            HashIndex = br.ReadUInt16();

            // Padding
            br.ReadBytes(2);

            SurfaceFlags = br.ReadInt32();
            Contents = br.ReadInt32();

            // Padding
            br.ReadBytes(4);
        }
    }
}
