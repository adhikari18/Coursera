using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphAlgo
{
    public class Node
    {
        public int Index { get; set; }
        public long Distance { get; set; }


        public Node(int index, long distance)
        {
            Index = index;
            Distance = distance;
        }
    }
    class Dijkstra
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

            var lastLine = Console.ReadLine().Split();
            var start = int.Parse(lastLine[0]);
            var end = int.Parse(lastLine[1]);

            var result = GetResult(adjacent, cost, start - 1, end - 1);
            Console.WriteLine(result);
            Console.ReadKey();
        }

        private static int GetResult(IReadOnlyList<List<int>> adjacent, IReadOnlyList<List<int>> cost, int source, int target)
        {
            var distances = new int[adjacent.Count];
            for (var i = 0; i < distances.Length; i++)
            {
                distances[i] = int.MaxValue;
            }
            distances[source] = 0;
            var queue = new List<Node> {new Node(source, distances[source])};
            while (queue.Count > 0)
            {
                var u = queue.OrderByDescending(x => x.Distance).First();
                queue.Remove(u);
                var uIndex = u.Index;
                foreach (var v in adjacent[uIndex])
                {
                    var vIndex = adjacent[uIndex].IndexOf(v);
                    if (distances[v] > distances[uIndex] + cost[uIndex][vIndex])
                    {
                        distances[v] = distances[uIndex] + cost[uIndex][vIndex];
                        queue.Add(new Node(v, distances[v]));
                    }
                }
            }
            if (distances[target] == int.MaxValue)
                return -1;
            return distances[target];
        }
    }
}
