using System;
using System.Collections.Generic;

namespace GraphAlgo
{
    class NegativeCycle
    {
        static void Main(string[] args)
        {
            var firstLine = Console.ReadLine().Split();
            var numberOfVertex = int.Parse(firstLine[0]);
            var numberOfEdges = int.Parse(firstLine[1]);

            var adjacent = new List<int>[numberOfVertex];
            var cost = new List<int>[numberOfVertex];

            for (var i = 0; i < numberOfVertex; i++)
            {
                adjacent[i] = new List<int>();
                cost[i] = new List<int>();
            }
            for (var i = 0; i < numberOfEdges; i++)
            {
                var lines = Console.ReadLine().Split();
                var x = int.Parse(lines[0]);
                var y = int.Parse(lines[1]);
                var w = int.Parse(lines[2]);

                adjacent[x - 1].Add(y - 1);
                cost[x - 1].Add(w);
            }

            var result = GetResult(adjacent, cost);
            Console.WriteLine(result);
            Console.ReadKey();
        }

        private static int GetResult(IReadOnlyList<List<int>> adj, IReadOnlyList<List<int>> cost)
        {
            var distances = new long[adj.Count];
            for (var i = 0; i < adj.Count; i++)
            {
                distances[i] = int.MaxValue;
            }
            distances[0] = 0;
            for (var i = 0; i < adj.Count; i++)
            {
                for (var u = 0; u < adj.Count; u++)
                {
                    foreach (var v in adj[u])
                    {
                        var vIndex = adj[u].IndexOf(v);
                        if (distances[v] > distances[u] + cost[u][vIndex])
                        {
                            distances[v] = distances[u] + cost[u][vIndex];
                            if (i == adj.Count - 1)
                            {
                                return 1;
                            }
                        }
                    }
                }
            }
            return 0;
        }
    }
}
