using Cerberus.Logic.Extensions;
using Cerberus.Logic.Games;
using Cerberus.Logic.Games.T4;
using Cerberus.Logic.Games.T6;
using Cerberus.Logic.Games.T7;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Cerberus.Logic
{
    public partial class FastFile : IDisposable
    {
        private bool _isDisposed = false;
        private readonly FileStream _openFileStream;

        private readonly DevType _dev = DevType.InfinityWard;
        private readonly char _compression = '0';
        private readonly string _type = "100";
        private readonly int _version = 5;
        private readonly ClientType _clientType = ClientType.Client;
        private readonly Platform _platform = Platform.PC;
        private readonly bool _isEncrypted = false;
        private readonly string _buildNumber = string.Empty;
        private XFile _xFileHeader = new XFile();
        private readonly string _ffName = string.Empty;

        public DevType GetDevType() => this._dev;
        public int GetVersion() => this._version;
        public ClientType GetClientType() => this._clientType;
        public Platform GetPlatform() => this._platform;
        public bool IsEncrypted() => this._isEncrypted;
        public string GetBuildNumber() => this._buildNumber;
        public XFile GetFileHeader() => this._xFileHeader;
        public string GetName() => this._ffName;

        internal void SetFileHeader(XFile file)
        {
            this._xFileHeader = file;
        }

        public CompressionLevel GetCompressionLevel() => this._compression switch
        {
            'u' => CompressionLevel.Fastest,
            '0' => CompressionLevel.Optimal,
            _ => throw new InvalidDataException("Invalid compression type.")
        };

        public XAssetList? AssetList;

        /// <summary>
        /// Create a new instance of the Fast File class to access data in the compressed fast files used in Call of Duty games
        /// </summary>
        /// <param name="path">A path to the input compressed Fast File</param>
        public FastFile(string path)
        {
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
                        this._clientType = (ClientType)br.ReadInt16();

                        if (this._clientType is ClientType.Server)
                        {
                            throw new InvalidDataException("Server Fast Files are unsupported");
                        }

                        this._platform = (Platform)br.ReadByte();

                        if (this._platform is not Platform.PC)
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
                        if (!Encoding.UTF8.GetString(br.ReadBytes(8)).Equals(BlackOps2.HeaderMagic))
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
                else if (this._dev is DevType.InfinityWard)
                {
                    if (this._version >= (int)IWFastFileVersion.T4WorldAtWar)
                    {
                        // Do Nothing
                    }
                }
            }
        }

        public void DecompressToFile(string outputPath)
        {
            Decompress(outputPath);
        }

        public void ReadFileEntries()
        {
            string filePath = $"{this._openFileStream.Name}.raw";
            Decompress(filePath);
            using (BinaryReader br = new BinaryReader(File.OpenRead(filePath)))
            {
                switch (this._dev)
                {
                    case DevType.Treyarch:
                    {
                        switch (this._version)
                        {
                            case (int)TAFastFileVersion.T6BlackOps2:
                            {
                                this.AssetList = BlackOps2.ReadFastFile(this, br);
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            File.Delete(filePath);
        }

        private void Decompress(string outputPath)
        {
            using (BinaryReader br = new BinaryReader(this._openFileStream, Encoding.UTF8, true))
            using (BinaryWriter bw = new BinaryWriter(File.Create(outputPath)))
            {
                switch (this._dev)
                {
                    case DevType.Treyarch:
                    {
                        switch (this._version)
                        {
                            case (int)TAFastFileVersion.T6BlackOps2:
                            {
                                BlackOps2.Decompress(this, br, bw);
                                break;
                            }

                            case (int)TAFastFileVersion.T7BlackOps3:
                            {
                                BlackOps3.Decompress(this, br, bw);
                                break;
                            }
                        }
                        break;
                    }

                    case DevType.InfinityWard:
                    {
                        switch (this._version)
                        {
                            case (int)IWFastFileVersion.T4WorldAtWar:
                            {
                                WorldAtWar.Decompress(br, bw);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
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