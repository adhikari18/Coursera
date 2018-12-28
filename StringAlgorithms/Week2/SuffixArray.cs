using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringAlgoCSharp
{
    class SuffixArray
    {
        static void Main(string[] args)
        {
            var text = Console.ReadLine();
            Console.WriteLine(string.Join(" ", GetSuffixArray(text)));
            Console.ReadKey();
        }

        static List<int> GetSuffixArray(string text)
        {
            var suffixes = new List<Tuple<string, int>>();
            for (var i = 0; i < text.Length; i++)
            {
                suffixes.Add(new Tuple<string, int>(text.Substring(i), i));
            }
            suffixes.Sort((x, y) => string.Compare(x.Item1, y.Item1, StringComparison.Ordinal));
            return suffixes.Select(t => t.Item2).ToList();
        }
    }
}
