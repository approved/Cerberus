namespace Cerberus.Logic.Games.T6
{
    public static partial class BlackOps2
    {
        public static readonly byte[] XboxSalsaKey = new byte[]
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

        public static readonly byte[] PlaystationSalsaKey = new byte[]
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

        public static readonly byte[] PCSalsaKey = new byte[]
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

        public const string HeaderMagic = "PHEEBs71";
    }

    public enum T6XAssetType
    {
        XModelPieces,
        PhysPreset,
        PhysConstraints,
        DestructibleDef,
        XAnimeParts,
        XModel,
        Material,
        TechniqueSet,
        Image,
        Sound,
        SoundPatch,
        ClipMap,
        ClipMapPVS,
        ComWorld,
        GameWorldSP,
        GameWorldMP,
        MapEnts,
        GFXWorld,
        LightDefe,
        UIMap,
        Font,
        FontIcon,
        MenuList,
        Menu,
        LocalizeEntry,
        Weapon,
        WeaponDef,
        WeaponVariant,
        WeaponFull,
        Attachment,
        AttachmentUnique,
        WeaponCamo,
        SoundDriverGlobals,
        Effects,
        ImpactFX,
        AIType,
        MPType,
        MPBody,
        MPHead,
        Character,
        XModelAlias,
        RawFile,
        StringTable,
        Leaderboard,
        XGlobals,
        DDL,
        Glasses,
        EmblemSet,
        ScriptParseTree,
        KeyValuePairs,
        VehicleDef,
        MemoryBlock,
        AddonMapEnts,
        Trace,
        SkinnedVerts,
        QDB,
        Slug,
        FootstepTable,
        FootstepEffectsTable,
        ZBarrier,
        Count,
        String,
        AssetList,
        Report,
        Depend,
        FullCount,
    }
}
