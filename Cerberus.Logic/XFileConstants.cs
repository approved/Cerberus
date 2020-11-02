using System;
using System.Collections.Generic;
using System.Text;

namespace Cerberus.Logic
{
    public partial class XFile
    {

    }

    public enum XFileBlocks
    {
        Temp = 0,

        // Modern Warfare 2
        IW4Physical = 1,
        IW4Runtime = 2,
        IW4Virtual = 3,
        IW4Large = 4,
        IW4Callback = 5,
        IW4XboxCount = 6,
        IW4Vertex = 6,
        IW4PlaystationCount = 7,
        IW4Index = 7,
        IW4PCCount = 8,

        // Ghosts
        IW6Physical = IW4Physical,
        IW6Runtime = IW4Runtime,
        IW6Virtual = IW4Virtual,
        IW6Large = IW4Large,
        IW6Callback = IW4Callback,
        IW6Script = 6,
        IW6Count = 7,

        // Black Ops 1
        T5Runtime = 1,
        T5LargeRuntime = 2,
        T5PhysicalRuntime = 3,
        T5Virtual = 4,
        T5Large = 5,
        T5Physical = 6,
        T5Count = 7,

        // Black Ops 2
        T6RuntimeVirtual = 1,
        T6RuntimePhysical = 2,
        T6DelayVirtual = 3,
        T6DelayPhysical = 4,
        T6Virtual = 5,
        T6Physical = T5Physical,
        T6StreamerReserve = 7,
        T6Count = 8,

        // Black Ops 3
        T7RuntimeVirtual = T6RuntimeVirtual,
        T7RuntimePhysical = T6RuntimePhysical,
        T7DelayVirtual = T6DelayVirtual,
        T7DelayPhysical = T6DelayPhysical,
        T7Virtual = T6Virtual,
        T7Physical = T6Physical,
        T7StreamreReserve = T6StreamerReserve,
        T7Streamer = 8,
        T7MemMapped = 9,
        T7Count = 10
    }
}
