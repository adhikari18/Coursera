using System;
using System.Collections.Generic;

namespace GraphAlgo
{
    class Bipartite
    {
        static void Main(string[] args)
        {
            var firstLine = Console.ReadLine().Split();
            var numberOfVertex = int.Parse(firstLine[0]);
            var numberOfEdges = int.Parse(firstLine[1]);

            var adjacent = new List<int>[numberOfVertex];

            for (var i = 0; i < numberOfVertex; i++)
            {
                adjacent[i] = new List<int>();
            }
            for (var i = 0; i < numberOfEdges; i++)
            {
                var lines = Console.ReadLine().Split();
                var x = int.Parse(lines[0]);
                var y = int.Parse(lines[1]);

                adjacent[x - 1].Add(y - 1);
                adjacent[y - 1].Add(x - 1);
            }

            var result = GetResult(adjacent);
            Console.WriteLine(result);
            Console.ReadKey();
        }

        private static int GetResult(IReadOnlyList<List<int>> adj)
        {
            var colors = new int[adj.Count];
            for (var i = 0; i < colors.Length; i++)
            {
                colors[i] = -1;
            }

            colors[0] = 1;

            var queue = new Queue<int>();
            queue.Enqueue(0);
            while (queue.Count > 0)
            {
                var u = queue.Dequeue();
                foreach (var v in adj[u])
                {
                    if (colors[v] == -1)
                    {
                        colors[v] = 1 - colors[u];
                        queue.Enqueue(v);
                    }
                    else if (colors[v] == colors[u])
                        return 0;
                }
            }
            return 1;
        }
    }
}
