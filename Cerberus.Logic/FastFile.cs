using Cerberus.Logic.Crypto;
using Cerberus.Logic.Extensions;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Cerberus.Logic
{
    public partial class FastFile : IDisposable
    {

        private bool _isDisposed = false;
        private readonly Stream _openFileStream;

        private readonly DevType _dev = DevType.InfinityWard;
        private readonly char _compression = '0';
        private readonly string _type = "100";
        private readonly int _version = 5;
        private readonly DBType _dbType = DBType.Client;
        private readonly DBPlatform _platform = DBPlatform.PC;
        private readonly bool _isEncrypted = false;
        private readonly string _buildNumber = string.Empty;
        private readonly XFile _xFileHeader = new XFile();
        private readonly string _ffName = string.Empty;

        public CompressionLevel GetCompressionLevel()
        {
            return this._compression switch
            {
                'u' => CompressionLevel.Fastest,
                '0' => CompressionLevel.Optimal,
                _ => throw new InvalidDataException("Invalid compression type.")
            };
        }

        /// <summary>
        /// Create a new instance of the Fast File class to access data in the compressed fast files used in Call of Duty games
        /// </summary>
        /// <param name="path">A path to the input compressed Fast File</param>
        public FastFile(string path)
        {
            Contract.Requires(File.Exists(path));

            this._openFileStream = File.OpenRead(path);

            using (BinaryReader br = new BinaryReader(this._openFileStream, Encoding.UTF8, true))
            {
                // Sets the local variable devType to the first 2 characters of the file
                string devType = new string(br.ReadChars(2));

                // Checks if we support this particular dev type. Prevents custom or unknown dev types from being processed
                if (!SupportedDevTypes.ContainsKey(devType))
                {
                    throw new InvalidDataException("File input contains unknown developer.");
                }

                // Set Dev equal to the developer type of this fast file
                this._dev = SupportedDevTypes[devType];

                // Check if the file is a Fast File
                if (!Encoding.UTF8.GetString(br.ReadBytes(2)).Equals(FastFileAbrv))
                {
                    throw new InvalidDataException("File input does not contain a valid Fast File");
                }

                // Get the compression character
                this._compression = br.ReadChar();

                // Unknown purpose always 100 or 000
                this._type = Encoding.UTF8.GetString(br.ReadBytes(3));
                
                // Get the fast file version
                this._version = br.ReadInt32();

                if (this._dev is DevType.Treyarch)
                {
                    // If we are on Black Ops 3 PC or higher
                    if (this._version >= (int)TAFastFileVersion.T7BlackOps3)
                    {
                        this._dbType = (DBType)br.ReadInt16();

                        if (this._dbType is DBType.Server)
                        {
                            throw new InvalidDataException("Server Fast Files are unsupported");
                        }

                        this._platform = (DBPlatform)br.ReadByte();

                        if (this._platform is not DBPlatform.PC)
                        {
                            // TODO: Support other platforms
                            throw new InvalidDataException($"Unsupported platform - {this._platform}");
                        }

                        this._isEncrypted = br.ReadByte() != 0;

                        if (this._isEncrypted)
                        {
                            // TODO: Support encrpytion keys
                            throw new InvalidDataException("File input is encryted");
                        }

                        // Unknown Data
                        br.ReadBytes(12);

                        // CheckSum
                        br.ReadInt32();
                        br.ReadInt32();
                        br.ReadInt32();
                        br.ReadInt32();

                        this._buildNumber = br.PeekNativeString(maxSize: 100);

                        // Seek to the end of the max build number size
                        br.BaseStream.Seek(100, SeekOrigin.Current);

                        this._xFileHeader = new XFile();
                        this._xFileHeader.Size = br.ReadInt64();

                        // Skip XFile Padding
                        br.ReadBytes(16);

                        // Create New Block Sizes array with the propery element count
                        this._xFileHeader.BlockSizes = new long[(int)XFileBlocks.T7Count];

                        // Loop through block sizes and assign them accordingly
                        for (int i = 0; i < (int)XFileBlocks.T7Count; i++)
                        {
                            this._xFileHeader.BlockSizes[i] = br.ReadInt64();
                        }

                        this._ffName = br.PeekNativeString(maxSize: 64);

                        // Seek to the end of the max ff name size
                        br.BaseStream.Seek(64, SeekOrigin.Current);

                        // Read Auth Signature - unneeded
                        br.ReadBytes(256);

                        // Padding?
                        br.ReadBytes(16);
                    }
                    else if(this._version >= (int)TAFastFileVersion.T6BlackOps2)
                    {
                        if (!Encoding.UTF8.GetString(br.ReadBytes(8)).Equals(T6HeaderMagic))
                        {
                            throw new InvalidDataException("Found Black Ops 2 FF, but could not read valid header magic");
                        }

                        // Padding
                        br.ReadInt32();

                        this._ffName = br.PeekNativeString(maxSize: 32);

                        // Seek to the end of the max ff name size
                        br.BaseStream.Seek(32, SeekOrigin.Current);

                        // Read Auth Signature - unneeded
                        br.ReadBytes(256);
                    }
                }
            }
        }

        public void DecompressToFile(string outputPath)
        {
            using (BinaryReader br = new BinaryReader(this._openFileStream, Encoding.UTF8, true))
            using (BinaryWriter bw = new BinaryWriter(File.Create(outputPath)))
            {
                if (this._dev is DevType.Treyarch)
                {
                    switch (this._version)
                    {
                        case (int)TAFastFileVersion.T6BlackOps2:
                        {
                            DecompressT6(br, bw);
                            break;
                        }

                        case (int)TAFastFileVersion.T7BlackOps3:
                        {
                            DecompressT7(br, bw);
                            break;
                        }
                    }
                }
            }
        }

        private void DecompressT6(BinaryReader br, BinaryWriter bw)
        {
            byte[] ivTable = new byte[16000];
            int[] ivCounter = new int[4] { 1, 1, 1, 1 };

            Salsa20.FillIVTable(ivTable, Encoding.UTF8.GetBytes(this._ffName));

            int sectionIndex = 0;
            Salsa20 salsa = new Salsa20
            {
                Key = this._platform switch
                {
                    DBPlatform.PC => T6PCSalsaKey,
                    DBPlatform.Playstation => T6PlaystationSalsaKey,
                    DBPlatform.Xbox => T6XboxSalsaKey,
                    _ => throw new NotImplementedException("Unknown platform found. Can not find salsa key.")
                }
            };

            int size;
            while ((size = br.ReadInt32()) != 0)
            {
                salsa.IV = Salsa20.GetIV(sectionIndex % 4, ivTable, ivCounter);

                ICryptoTransform? decryptor = salsa.CreateDecryptor();

                byte[] decryptedData = decryptor.TransformFinalBlock(br.ReadBytes(size), 0, size);

                bw.Write(Utility.Deflate(decryptedData).ToArray());

                using (SHA1 sha1 = SHA1.Create())
                {
                    Salsa20.UpdateIVTable(sectionIndex % 4, sha1.ComputeHash(decryptedData), ivTable, ivCounter);
                }

                sectionIndex++;
            }
        }

        private void DecompressT7(BinaryReader br, BinaryWriter bw)
        {
            long consumed = 0;
            int blockCount = 0;
            while (consumed < this._xFileHeader.Size)
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
            Console.WriteLine(blockCount);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._isDisposed) return;

            if (disposing)
            {
                if (this._openFileStream is not null)
                {
                    this._openFileStream.Close();
                }
            }

            this._isDisposed = true;
        }
    }
}