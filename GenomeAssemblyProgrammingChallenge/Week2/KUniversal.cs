//(Max time used: 0.56/1.50, max memory used: 35295232/536870912.)
using System;
using System.Collections.Generic;

namespace GenomeProgrammingChallenge
{
    class KUniversal
    {
        static void Main(string[] args)
        {
            var input = int.Parse(Console.ReadLine());
            Console.WriteLine(GetResult(input));
        }

        static string GetResult(int input)
        {
            var lastIndex = input - 1;
            var lastBinary = "".PadLeft(input, '1');
            var lastBinaryInt = Convert.ToInt32(lastBinary, 2);

            var lastBefore = "1" + "".PadLeft(lastIndex, '0');
            var firstBinary = "".PadLeft(input, '0');
            var nodes = new Dictionary<string, List<string>>();
            for (var i = 0; i <= lastBinaryInt; i++)
            {
                var binaryI = Convert.ToString(i, 2).PadLeft(input, '0');
                if (binaryI != lastBefore && binaryI != firstBinary)
                {
                    var s = binaryI.Substring(0, lastIndex);
                    var e = binaryI.Substring(1, input - 1);
                    if (!nodes.ContainsKey(s))
                        nodes.Add(s, new List<string> { e });
                    else nodes[s].Add(e);
                    if (!nodes.ContainsKey(e))
                        nodes.Add(e, new List<string> { s });
                    else nodes[e].Add(s);
                }
            }

            var start = "".PadLeft(input - 1, '0');
            var list = new List<string> {start};
            var current = start;

            while (nodes[current].Count > 0)
            {
                var suffix = current.Substring(1);
                var nextChar = nodes[current].Contains(suffix + "1") ? "1" : "0";
                list.Add(suffix + nextChar);
                nodes[current].Remove(suffix + nextChar);
                nodes[suffix + nextChar].Remove(current);
                current = suffix + nextChar;
            }

            var result = "0";
            list.ForEach(item => result += item[0]);
            return result;
        }
    }
}
