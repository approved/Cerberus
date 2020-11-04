using Cerberus.Logic;
using System;

namespace Cerberus.CLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FastFile ffile = new FastFile(args[0]);
            //ffile.DecompressToFile($"{args[0]}.raw");

            ffile.ReadFileEntries();
            Console.WriteLine($"Found: \n" +
                $"\t{ffile.AssetList.StringCount} Strings");
            foreach (string str in ffile.AssetList.StringList)
            {
                Console.WriteLine($"\t\t{str}");
            }

            Console.WriteLine($"{ffile.AssetList.AssetCount} Assets");
            Console.WriteLine($"{ffile.AssetList.Entries.Count} Entries");
        }
    }
}
