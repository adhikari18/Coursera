using System;
using System.Collections.Generic;

namespace GraphAlgo
{
    class Acyclicity
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
            Console.WriteLine(GetCycles(adjacent));
            Console.ReadKey();
        }

        private static int GetCycles(List<int>[] adjacent)
        {
            var visited = new int[adjacent.Length];
            var recursiveStack = new int[adjacent.Length];
            for (var i = 0; i < adjacent.Length; i++)
            {
                if (visited[i] == 0 && DFS(adjacent, i, visited, recursiveStack) == 1)
                {
                    return 1;
                }
            }
            return 0;
        }

        private static int DFS(List<int>[] adjacent, int x, int[] visited, int[] recursiveStack)
        {
            visited[x] = 1;
            recursiveStack[x] = 1;
            for (var i = 0; i < adjacent[x].Count; i++)
            {
                if (visited[adjacent[x][i]] != 1 
                    && DFS(adjacent, adjacent[x][i], visited, recursiveStack) == 1)
                {
                    return 1;
                }

                if(recursiveStack[adjacent[x][i]] == 1)
                {
                    return 1;
                }
            }
            recursiveStack[x] = 0;
            return 0;
        }
    }
}
