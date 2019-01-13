using System;
using System.Collections.Generic;

namespace GraphAlgo
{
    class BFS
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

            var lastLine = Console.ReadLine().Split();
            var start = int.Parse(lastLine[0]);
            var end = int.Parse(lastLine[1]);

            var result = GetResult(adjacent, start - 1, end - 1);
            Console.WriteLine(result);
            Console.ReadKey();
        }

        private static int GetResult(List<int>[] adj, int x, int y)
        {
            if (x == y)
            {
                return 0;
            }

            var distances = new int[adj.Length];
            for (var i = 0; i < distances.Length; i++)
            {
                distances[i] = int.MaxValue;
            }
            distances[x] = 0;
            var queue = new Queue<int>();
            queue.Enqueue(x);
            while (queue.Count > 0)
            {
                var u = queue.Dequeue();
                foreach (var v in adj[u])
                {
                    if (distances[v] == int.MaxValue)
                    {
                        queue.Enqueue(v);
                        distances[v] = distances[u] + 1;
                    }
                }
            }

            if (distances[y] != int.MaxValue)
            {
                return distances[y];
            }

            return -1;
        }
    }
}
