using Cerberus.Logic.Algo;
using System;
using System.IO;
using System.Text;

namespace Cerberus.Logic.Extensions
{
    public static class BinaryReaderExtensions
    {
        private const int BufferSize = 1024 * 1024;

        public static string PeekNativeString(this BinaryReader br, long offset = 0, SeekOrigin seekOrigin = SeekOrigin.Current, int maxSize = -1)
        {
            long currentPos = br.BaseStream.Position;

            string str = ReadNativeString(br, offset, seekOrigin, maxSize);

            br.BaseStream.Seek(currentPos, SeekOrigin.Begin);
            return str;
        }

        public static string ReadNativeString(this BinaryReader br, long offset = 0, SeekOrigin seekOrigin = SeekOrigin.Current, int maxSize = -1)
        {
            br.BaseStream.Seek(offset, seekOrigin);

            byte b;
            int size = 0;

            StringBuilder sb = new StringBuilder();
            while ((b = br.ReadByte()) != 0 && size++ != maxSize)
            {
                sb.Append(Convert.ToChar(b));
            }

            return sb.ToString();
        }

        public static string PeekNativeUTF16String(this BinaryReader br, long offset = 0, SeekOrigin seekOrigin = SeekOrigin.Current, int maxSize = -1)
        {
            long currentPos = br.BaseStream.Position;

            string str = ReadNativeUTF16String(br, offset, seekOrigin, maxSize);

            br.BaseStream.Seek(currentPos, SeekOrigin.Begin);
            return str;
        }

        public static string ReadNativeUTF16String(this BinaryReader br, long offset = 0, SeekOrigin seekOrigin = SeekOrigin.Current, int maxSize = -1)
        {
            br.BaseStream.Seek(offset, seekOrigin);

            ushort b;
            int size = 0;

            StringBuilder sb = new StringBuilder();
            while ((b = br.ReadUInt16()) != 0 && size++ != maxSize)
            {
                sb.Append(Convert.ToChar(b));
            }

            return sb.ToString();
        }

        public static long[] FindString(this BinaryReader br, string needle, bool firstOccurence = false) => FindBytes(br, Encoding.ASCII.GetBytes(needle), firstOccurence);

        public static long[] FindBytes(this BinaryReader br, byte[] needle, bool firstOccurence = false)
        {
            int[] res = BoyerMooreHorspool.Search(needle, br.ReadBytes(BufferSize), firstOccurence);

            if (res.Length > 0)
            {
                return Array.ConvertAll(res, x => (long)x);
            }

            return Array.Empty<long>();
        }
    }
}
