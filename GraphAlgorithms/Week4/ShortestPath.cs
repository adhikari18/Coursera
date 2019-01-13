using System;
using System.Collections.Generic;

namespace GraphAlgo
{
    class ShortestPaths
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

            var s = int.Parse(Console.ReadLine()) - 1;

            long[] distance = new long[numberOfVertex];
            int[] reachable = new int[numberOfVertex];
            int[] shortest = new int[numberOfVertex];

            for (int i = 0; i < numberOfVertex; i++)
            {
                distance[i] = long.MaxValue;
                reachable[i] = 0;
                shortest[i] = 1;
            }

            GetResult(adjacent, cost, s, distance, reachable, shortest);

            for (int i = 0; i < numberOfVertex; i++)
            {
                if (reachable[i] == 0)
                {
                    Console.WriteLine("*");
                }
                else if (shortest[i] == 0)
                {
                    Console.WriteLine("-");
                }
                else
                {
                    Console.WriteLine(distance[i]);
                }
            }
            Console.ReadKey();
        }

        private static void GetResult(IReadOnlyList<List<int>> adj, IReadOnlyList<List<int>> cost, int s, long[] distance, int[] reachable, int[] shortest)
        {
            distance[s] = 0;
            reachable[s] = 1;
            Queue<int> queue = new Queue<int>();
            for (int i = 0; i < adj.Count; i++)
            {
                for (int u = 0; u < adj.Count; u++)
                {
                    foreach (int v in adj[u])
                    {
                        int v_index = adj[u].IndexOf(v);
                        if (distance[u] != long.MaxValue && distance[v] > distance[u] + cost[u][v_index])
                        {
                            distance[v] = distance[u] + cost[u][v_index];
                            reachable[v] = 1;
                            if (i == adj.Count - 1)
                            {
                                queue.Enqueue(v);
                            }
                        }
                    }
                }
            }

            int[] visited = new int[adj.Count];
            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                visited[u] = 1;
                if (u != s)
                    shortest[u] = 0;
                foreach (int v in adj[u])
                {
                    if (visited[v] == 0)
                    {
                        queue.Enqueue(v);
                        visited[v] = 1;
                        shortest[v] = 0;
                    }
                }
            }
            distance[s] = 0;
        }
    }
}
