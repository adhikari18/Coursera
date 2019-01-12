using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphAlgo
{
    class ConnectedComponents
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
            Console.WriteLine(GetConnectedComponents(adjacent));
            Console.ReadKey();
        }

        private static int GetConnectedComponents(List<int>[] adjacent)
        {
            int result = 0;
            int[] visited = new int[adjacent.Length];
            for (int i = 0; i < adjacent.Length; i++)
            {
                if (visited[i] == 0)
                {
                    Explore(adjacent, i, visited);
                    result++;
                }
            }
            return result;
        }

        private static void Explore(List<int>[] adjacent, int x, int[] visited)
        {
            visited[x] = 1;
            for (int i = 0; i < adjacent[x].Count(); i++)
            {
                if (visited[adjacent[x][i]] != 1)
                {
                    Explore(adjacent, adjacent[x][i], visited);
                }
            }
        }
    }
}
