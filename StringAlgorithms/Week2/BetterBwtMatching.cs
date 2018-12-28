using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace StringAlgoCSharp
{
    class BwtMatching
    {
        static void Main(string[] args)
        {
            var bwtText = Console.ReadLine();
            var starts = new Dictionary<char, int>();
            var occCountsBefore = new Dictionary<char, int[]>();
            PreprocessBwt(bwtText, starts, occCountsBefore);

            var patternCount = int.Parse(Console.ReadLine());
            var patternsInput = Console.ReadLine().Split();
            var patterns = new List<string>();
            var result = new int[patternCount];
            for (var i = 0; i < patternCount; i++)
            {
                patterns.Add(patternsInput[i]);
                result[i] = CountOccurrences(patterns[i], bwtText, starts, occCountsBefore);
            }

            Console.WriteLine(string.Join(" ", result));
            Console.ReadKey();
        }

        private static int CountOccurrences(string pattern, string bwt, Dictionary<char, int> starts, Dictionary<char, int[]> occCountsBefore)
        {
            var top = 0;
            var bottom = bwt.Length - 1;
            var patternLength = pattern.Length;
            while (top <= bottom)
            {
                if (patternLength > 0)
                {
                    patternLength--;
                    var letter = pattern[patternLength];
                    int[] val;
                    if (!occCountsBefore.TryGetValue(letter, out val)) return 0;

                    var topOccurence = occCountsBefore[letter][top];
                    var bottomOccurence = occCountsBefore[letter][bottom + 1];
                    var start = starts[letter];
                    if (bottomOccurence > topOccurence)
                    {
                        top = start + topOccurence;
                        bottom = start + bottomOccurence - 1;
                    }
                    else return 0;
                }
                else return bottom - top + 1;
            }
            return 0;
        }
        private static void PreprocessBwt(string bwt, Dictionary<char, int> starts, Dictionary<char, int[]> occCountsBefore)
        {
            var sortBwt = bwt.ToCharArray();

            Array.Sort(sortBwt);
            for (var i = 0; i < sortBwt.Length; i++)
            {
                int val;
                if (!starts.TryGetValue(sortBwt[i], out val)) starts.Add(sortBwt[i], i);
            }

            foreach (var ch in starts.Keys)
            {
                occCountsBefore.Add(ch, new int[bwt.Length + 1]);
            }
            
            for (var i = 1; i < bwt.Length + 1; i++)
            {
                var current = bwt[i - 1];
                foreach (var characterEntry in occCountsBefore)
                {
                    characterEntry.Value[i] = characterEntry.Value[i - 1] + (characterEntry.Key == current ? 1 : 0);
                }
            }

        }
    }
}
