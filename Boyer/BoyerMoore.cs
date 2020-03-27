using System;
using System.Collections.Generic;
using System.Text;

namespace Boyer
{
    static class BoyerMoore
    {
        public static int[] SearchString(byte[] bytes, string pattern)
        {
            List<int> result = new List<int>();
            var bytesToSearch = Encoding.Unicode.GetBytes(pattern);

            int m = bytesToSearch.Length;
            int n = bytes.Length;

            int[] badChar = new int[256];

            BadCharHeuristic(bytesToSearch, m, ref badChar);

            int s = 0;
            while (s <= (n - m))
            {
                int j = m - 1;

                while (j >= 0 && bytesToSearch[j] == bytes[s + j])
                    --j;

                if (j < 0)
                {
                    result.Add(s);
                    s += (s + m < n) ? m - badChar[bytes[s + m]] : 1;
                }
                else
                {
                    s += Math.Max(1, j - badChar[bytes[s + j]]);
                }
            }

            return result.ToArray();
        }

        private static void BadCharHeuristic(byte[] str, int size, ref int[] badChar)
        {
            int i;

            for (i = 0; i < 256; i++)
                badChar[i] = -1;

            for (i = 0; i < size; i++)
                badChar[(int)str[i]] = i;
        }
    }
}
