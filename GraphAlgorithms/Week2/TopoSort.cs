using System;
using System.Collections.Generic;

namespace GraphAlgo
{
    class TopoSort
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
            }

            var result = GetResult(adjacent);
            for (var i = 0; i < result.Count; i++)
            {
                result[i] = result[i] + 1;
            }

            Console.WriteLine(string.Join(" ",result));
            Console.ReadKey();
        }

        private static List<int> GetResult(List<int>[] adjacent)
        {
            var visited = new int[adjacent.Length];
            var order = new List<int>();
            for (var i = 0; i < adjacent.Length; i++)
            {
                if (visited[i] == 0)
                {
                    DFS(adjacent, visited, order, i);
                }
            }
            return order;
        }

        private static void DFS(List<int>[] adjacent, int[] visited, List<int> order, int i)
        {
            visited[i] = 1;

            for (var j = 0; j < adjacent[i].Count; j++)
            {
                if (visited[adjacent[i][j]] == 0)
                {
                    DFS(adjacent, visited, order, adjacent[i][j]);
                }
            }

            order.Insert(0, i);
        }
    }
}
