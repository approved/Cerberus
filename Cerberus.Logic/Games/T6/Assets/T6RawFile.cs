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

        public static void Load(XAssetList assetList, BinaryReader br)
        {
            T6RawFile entry = new T6RawFile()
            {
                NamePointer = br.ReadInt32(),
                Size = br.ReadInt32(),
                DataPointer = br.ReadInt32()
            };

            if (entry.NamePointer != -1)
            {
                throw new InvalidDataException("XAsset name pointer entries are not supported");
            }

            entry.Name = br.ReadNativeString(0, SeekOrigin.Current, 128);

            if (entry.DataPointer != -1)
            {
                throw new InvalidDataException("XAsset data pointer entries are not supported");
            }

            entry.SetData(br.ReadBytes(entry.Size + 1));

            assetList.Entries.Add(entry);
        }
    }
}
