using System;
using System.Collections.Generic;
using System.Text;

namespace Cerberus.Logic
{
    // Thanks to this helpful resource for many insights https://wiki.orbismodding.com/index.php/Category:FastFiles
    public partial class FastFile
    {
        private static readonly Dictionary<string, DevType> SupportedDevTypes = new Dictionary<string, DevType>()
        {
            { "IW", DevType.InfinityWard },
            { "TA", DevType.Treyarch },
            { "SL", DevType.Sledgehammer }
        };

        private static readonly byte[] T5XboxSalsaKey = new byte[]
        {
            0x1a, 0xc1, 0xd1, 0x2d,
            0x52, 0x7c, 0x59, 0xb4,
            0x0e, 0xca, 0x61, 0x91,
            0x20, 0xff, 0x82, 0x17,
            0xcc, 0xff, 0x09, 0xcd,
            0x16, 0x89, 0x6f, 0x81,
            0xb8, 0x29, 0xc7, 0xf5,
            0x27, 0x93, 0x40, 0x5d
        };

        private static readonly byte[] T5PlaystationSalsaKey = new byte[]
        {
            0x0C, 0x99, 0xB3, 0xDD,
            0xB8, 0xD6, 0xD0, 0x84,
            0x5D, 0x11, 0x47, 0xE4,
            0x70, 0xF2, 0x8A, 0x8B,
            0xF2, 0xAE, 0x69, 0xA8,
            0xA9, 0xF5, 0x34, 0x76,
            0x7B, 0x54, 0xE9, 0x18,
            0x0F, 0xF5, 0x53, 0x70
        };

        private static readonly byte[] T6XboxSalsaKey = new byte[]
        {
            0x0E, 0x50, 0xF4, 0x9F,
            0x41, 0x23, 0x17, 0x09,
            0x60, 0x38, 0x66, 0x56,
            0x22, 0xDD, 0x09, 0x13,
            0x32, 0xA2, 0x09, 0xBA,
            0x0A, 0x05, 0xA0, 0x0E,
            0x13, 0x77, 0xCE, 0xDB,
            0x0A, 0x3C, 0xB1, 0xD3
        };

        private static readonly byte[] T6PlaystationSalsaKey = new byte[]
        {
            0xC8, 0x0B, 0x0E, 0x0C,
            0x15, 0x4B, 0xFF, 0x91,
            0x76, 0xA0, 0xC5, 0xC8,
            0xD2, 0x4F, 0xA5, 0xE3,
            0xEE, 0x09, 0xEE, 0x90,
            0x6F, 0x72, 0x90, 0x80,
            0xA3, 0x92, 0x75, 0xFD,
            0x3E, 0xA7, 0x13, 0x39
        };

        private static readonly byte[] T6PCSalsaKey = new byte[]
        {
            0x64, 0x1D, 0x8A, 0x2F,
            0xE3, 0x1D, 0x3A, 0xA6,
            0x36, 0x22, 0xBB, 0xC9,
            0xCE, 0x85, 0x87, 0x22,
            0x9D, 0x42, 0xB0, 0xF8,
            0xED, 0x9B, 0x92, 0x41,
            0x30, 0xBF, 0x88, 0xB6,
            0x5E, 0xDC, 0x50, 0xBE
        };

        private const string T6HeaderMagic = "PHEEBs71";
        private const string FastFileAbrv = "ff";
    }

    public enum DevType
    {
        InfinityWard,
        Treyarch,
        Sledgehammer
    }

    public enum DBType : short
    {
        Server = 0x0000,
        Client = 0x0100
    };

    public enum DBPlatform : byte
    {
        PC,
        Xbox,
        Playstation
    }

    /// <summary>
    /// Enum of IW denoted fast file versions for PC
    /// </summary>
    public enum IWFastFileVersion
    {
        IW5ModernWarfare3 = 1,
        IW3ModernWarfare = 5,
        IW4ModernWarfare2 = 276,
        T4WorldAtWar = 387,
        T5BlackOps = 473,
        IW6Ghosts = 565,
        IW7InfiniteWarfare = 1619,
    }

    /// <summary>
    /// Enum of TA denoted fast file versions for PC
    /// </summary>
    public enum TAFastFileVersion
    {
        T6BlackOps2 = 147,
        T7BlackOps3 = 593,
    }

    /// <summary>
    /// Enum of SL denoted fast file versions for PC
    /// </summary>
    public enum SLFastFileVersion
    {
        H1ModernWarfare = 66,
    }
}
