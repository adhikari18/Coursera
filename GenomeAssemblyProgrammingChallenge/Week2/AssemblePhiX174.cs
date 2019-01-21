using System;
using System.Collections.Generic;

namespace GenomeProgrammingChallenge
{
    class PhiX174Assembly
    {
        private static int n = 5396;
        static void Main(string[] args)
        {
            var inputs = new List<string>();
            for (int i = 0; i < n; i++)
            {
                inputs.Add(Console.ReadLine());
            }
            Console.WriteLine(GetResult(inputs));
            Console.ReadKey();
        }

        static string GetResult(List<string> inputs)
        {
            var dict = new Dictionary<string, List<Tuple<string, int>>>();
            for (var i = 0; i < n; i++)
            {
                var read = inputs[i];
                var key1 = read.Substring(0, read.Length - 1);
                var key2 = read.Substring(1);
                if (dict.ContainsKey(key1))
                {
                    dict[key1].Add(new Tuple<string, int>(key2, i));
                }
                else
                {
                    dict.Add(key1, new List<Tuple<string, int>>{new Tuple<string, int>(key2, i)});
                }
            }

            var path = new List<string> {inputs[0].Substring(0, inputs[0].Length - 1)};
            var visited = new List<int>();
            var newCycle = new List<string>();
            while (visited.Count < inputs.Count)
            {
                int i;
                for (i = 0; i < path.Count; i++)
                {
                    var node = path[i];
                    var allVisited = true;
                    foreach (var nextPoint in dict[node])
                    {
                        if (visited.Contains(nextPoint.Item2)) continue;
                        allVisited = false;
                        break;
                    }

                    if (allVisited) continue;
                    newCycle = new List<string>{node};
                    var current = node;
                    var findNext = true;
                    while (findNext)
                    {
                        findNext = false;
                        foreach (var nextPoint in dict[current])
                        {
                            if (visited.Contains(nextPoint.Item2)) continue;
                            visited.Add(nextPoint.Item2);
                            newCycle.Add(nextPoint.Item1);
                            current = nextPoint.Item1;
                            findNext = true;
                            break;
                        }
                    }
                    break;
                }

                var temp = new List<string>();
                var first = path.GetRange(0, i);
                var last = new List<string>();
                if (path.Count > i + 1) 
                    last = path.GetRange(i + 1, path.Count - (i+1));
                temp.AddRange(first);
                temp.AddRange(newCycle);
                temp.AddRange(last);
                path = temp;
            }

            var cycle = string.Empty;
            foreach (var node in path)
            {
                cycle += node[0];
            }
            return cycle.Substring(0, cycle.Length - 1);
        }
    }
}
