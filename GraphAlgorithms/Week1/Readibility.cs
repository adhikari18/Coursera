using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphAlgo
{
    class Reachability
    {
        static void Main(string[] args)
        {
            var firstLine = Console.ReadLine().Split();
            var numberOfVertex = int.Parse(firstLine[0]);
            var numberOfEdges = int.Parse(firstLine[1]);

            var adjacent = new List<int>[numberOfVertex];

            for (int i = 0; i < numberOfVertex; i++)
            {
                adjacent[i] = new List<int>();
            }
            for (int i = 0; i < numberOfEdges; i++)
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
            Console.WriteLine(Reach(adjacent, start - 1, end - 1));
            Console.ReadKey();
        }

        private static int Reach(List<int>[] adjacent, int x, int y)
        {
            int[] visited = new int[adjacent.Length];
            return Explore(adjacent, x, y, visited);
        }

        private static int Explore(List<int>[] adjacent, int x, int y, int[] visited)
        {
            if (x == y)
            {
                return 1;
            }
            visited[x] = 1;
            for (int i = 0; i < adjacent[x].Count(); i++)
            {
                if (visited[adjacent[x][i]] != 1)
                {
                    if (Explore(adjacent, adjacent[x][i], y, visited) == 1)
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }
    }
}
