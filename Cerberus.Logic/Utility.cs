using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Cerberus.Logic
{
    internal static class Utility
    {
        /// <summary>
        /// Computes the number of bytes require to pad this value
        /// </summary>
        public static int ComputePadding(int value, int alignment) => ((value + (alignment - 1)) & ~(alignment - 1)) - value;

        /// <summary>
        /// Aligns the value to the given alignment
        /// </summary>
        public static int AlignValue(int value, int alignment) => (value + (alignment - 1)) & ~(alignment - 1);

        /// <summary>
        /// Counts the number of lines in the given string
        /// </summary>
        public static int GetLineCount(string str) => str.Count(x => x == '\n');

        public static string SanitiseString(string value) => value.Replace("/", "\\").Replace("\b", "\\b");

        public static MemoryStream Deflate(byte[] data)
        {
            using(MemoryStream result = new MemoryStream())
            using (DeflateStream deflateStream = new DeflateStream(new MemoryStream(data), CompressionMode.Decompress))
            {
                deflateStream.CopyTo(result);
                result.Flush();
                result.Seek(0, SeekOrigin.Current);
                return result;
            }
        }
    }
}
