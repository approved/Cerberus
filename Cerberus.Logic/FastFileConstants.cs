using System.Collections.Generic;

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

        private const string FastFileAbrv = "ff";
    }

    public enum DevType
    {
        InfinityWard,
        Treyarch,
        Sledgehammer
    }

    public enum ClientType : short
    {
        Server = 0x0000,
        Client = 0x0100
    };

    public enum Platform : byte
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
