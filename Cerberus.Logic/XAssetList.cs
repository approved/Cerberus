using System;
using System.Collections.Generic;

namespace Cerberus.Logic
{
    public class XAssetList
    {
        public long StringCount;
        public long StringPointer;
        public long DependsCount;
        public long DependsPointer;
        public long AssetCount;
        public long AssetPointer;
        public int[] StringPointerArray = Array.Empty<int>();
        public List<string> StringList = new List<string>();

        public int[] DependsPointerArray = Array.Empty<int>();
        public List<string> DependsList = new List<string>();

        public List<IAsset> AssetList = new List<IAsset>();
        public List<object> Entries = new List<object>();
    }
}
