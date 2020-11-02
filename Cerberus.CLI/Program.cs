using Cerberus.Logic;

namespace Cerberus.CLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new FastFile(args[0]).DecompressToFile($"{args[0]}.raw");
        }
    }
}
