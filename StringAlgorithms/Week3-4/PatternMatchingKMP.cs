using System;
using System.Collections.Generic;

namespace StringAlgoCSharp
{
    static class PatternMatching
    {
        static void Main(string[] args)
        {
            var pattern = Console.ReadLine();
            var text = Console.ReadLine();
            Console.WriteLine(string.Join(" ", GetOccurrences(pattern, text)));
            Console.ReadKey();
        }

        private static IEnumerable<int> GetOccurrences(string pattern, string txt)
        {
            var result = new List<int>();
            var patLen = pattern.Length;
            var txtLen = txt.Length;

            var lps = ComputeLpsArray(pattern, patLen);

            var i = 0;
            var j = 0;
            while (i < txtLen)
            {
                if (pattern[j] == txt[i])
                {
                    j++;
                    i++;
                }
                if (j == patLen)
                {
                    result.Add(i - j);
                    j = lps[j - 1];
                }
                else if (i < txtLen && pattern[j] != txt[i])
                {
                    if (j != 0)
                    {
                        j = lps[j - 1];
                    }
                    else
                    {
                        i = i + 1;
                    }
                }
            }

            return result;
        }

        private static int[] ComputeLpsArray(string pattern, int patternLen)
        {
            var lps = new int[pattern.Length];
            var len = 0;
            var i = 1;
            lps[0] = 0; 

            while (i < patternLen)
            {
                if (pattern[i] == pattern[len])
                {
                    len++;
                    lps[i] = len;
                    i++;
                }
                else
                {
                    if (len != 0)
                    {
                        len = lps[len - 1];
                    }
                    else
                    {
                        lps[i] = len;
                        i++;
                    }
                }
            }

            return lps;
        }
    }
}
