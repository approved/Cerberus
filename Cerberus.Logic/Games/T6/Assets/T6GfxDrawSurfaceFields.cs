using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6GfxDrawSurfaceFields
    {
        private ulong _fieldData;
        public ulong ObjectId               => Utility.GetBits(this._fieldData,  0, 16);
        public ulong CustomIndex            => Utility.GetBits(this._fieldData, 16,  9);
        public ulong ReflectionProbeIndex   => Utility.GetBits(this._fieldData, 25,  5);
        public ulong DLightMask             => Utility.GetBits(this._fieldData, 30,  2);
        public ulong MaterialSortedIndex    => Utility.GetBits(this._fieldData, 32, 12);
        public ulong PrimaryLightIndex      => Utility.GetBits(this._fieldData, 44,  8);
        public ulong SurfaceType            => Utility.GetBits(this._fieldData, 52,  4);
        public ulong Prepass                => Utility.GetBits(this._fieldData, 56,  2);
        public ulong PrimarySortKey         => Utility.GetBits(this._fieldData, 58,  6);

        public bool HasData() => _fieldData > 0;

        public T6GfxDrawSurfaceFields(BinaryReader br)
        {
            this._fieldData = br.ReadUInt64();
        }
    }
}