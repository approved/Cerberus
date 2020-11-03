using System;
using System.IO;

namespace Cerberus.Logic.Games
{
    public partial class BlackOps3
    {
        public static void Decompress(FastFile fastfile, BinaryReader br, BinaryWriter bw)
        {
            long consumed = 0;
            int blockCount = 0;
            while (consumed < fastfile.GetFileHeader().Size)
            {
                // Compressed Size
                int blockCompSize = br.ReadInt32();

                // Decompressed Size
                int blockDecompSize = br.ReadInt32();

                // Block Size
                int blockSize = br.ReadInt32();

                // Block Position
                int blockPos = br.ReadInt32();

                if (blockPos != br.BaseStream.Position - 16)
                {
                    throw new InvalidDataException("Stream Position does not match expected position");
                }

                if (blockDecompSize == 0)
                {
                    br.BaseStream.Seek(Utility.ComputePadding((int)br.BaseStream.Position, 0x800000), SeekOrigin.Current);
                    continue;
                }

                br.BaseStream.Seek(2, SeekOrigin.Current);

                byte[] block = Utility.Deflate(br.ReadBytes(blockCompSize - 2)).ToArray();
                if (block.Length != blockDecompSize)
                {
                    // Should never execute
                    throw new IndexOutOfRangeException($"Decompressed block size ({block.Length}) did not match expected size ({blockDecompSize})");
                }
                bw.Write(block);

                consumed += block.Length;

                // Making sure to align our reader to end of the current block
                br.BaseStream.Seek(blockPos + 16 + blockSize, SeekOrigin.Begin);
                blockCount++;
            }
        }
    }
}
