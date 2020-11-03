using System.Collections.Generic;

namespace Cerberus.Logic.Algo
{
    public static class BoyerMooreHorspool
    {
        private static int[] Preprocess(byte[] pattern)
        {
            
            int[] table = new int[256];
            for (int i = 0; i < table.Length; i++)
            {
                table[i] = pattern.Length;
            }

            for (int i = 0; i < pattern.Length; i++)
            {
                table[pattern[i]] = pattern.Length - 1 - i;
            }

            return table;
        }

        private static bool IsSame(byte[] a, byte[] b, int len)
        {
            int i = 0;
            if (a[len - 1] == b[len - 1])
            {
                while (a[i] == b[i])
                {
                    if (i == len - 2)
                        return true;
                    i++;
                }
            }

            return false;
        }

        public static int[] Search(byte[] needle, byte[] haystack, bool singleMatch = false)
        {
            List<int> results = new List<int>();
            int[] table = Preprocess(needle);
            int skip = 0;

            while (haystack.Length - skip >= needle.Length)
            {
                if (IsSame(haystack, needle, needle.Length))
                {
                    if (singleMatch)
                    {
                        return new int[] { skip };
                    }
                    results.Add(skip);
                }
                else
                {
                    skip += table[haystack[skip + needle.Length - 1]];
                }
            }

            return results.ToArray();
        }
    }
}
