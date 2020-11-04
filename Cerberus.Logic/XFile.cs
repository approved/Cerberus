using System;

namespace Cerberus.Logic
{
    public partial class XFile
    {
        public long Size;
        public long ExternalSize;
        public long[] BlockSizes = Array.Empty<long>();
    }
}
