using Cerberus.Logic.Extensions;
using System;
using System.IO;

namespace Cerberus.Logic.Games.T6.Assets
{
    public class T6RawFile
    {
        public int NamePointer;
        public int DataPointer;
        public int Size;
        public string Name = string.Empty;
        private byte[] _data = Array.Empty<byte>();

        public byte[] GetData() => this._data;
        public void SetData(byte[] data)
        {
            if (data.Length != Size + 1)
            {
                throw new ArgumentOutOfRangeException("Input data length did not match expected size");
            }

            this._data = data;
        }

        public T6RawFile(BinaryReader br)
        {
            this.NamePointer = br.ReadInt32();
            this.Size = br.ReadInt32();
            this.DataPointer = br.ReadInt32();

            if (this.NamePointer != -1)
            {
                throw new InvalidDataException("XAsset name pointer entries are not supported");
            }

            this.Name = br.ReadNativeString(0, SeekOrigin.Current, 128);

            if (this.DataPointer != -1)
            {
                throw new InvalidDataException("XAsset data pointer entries are not supported");
            }

            this.SetData(br.ReadBytes(this.Size + 1));
        }

        public static void Load(XAssetList assetList, BinaryReader br)
        {
            assetList.Entries.Add(new T6RawFile(br));
        }
    }
}
