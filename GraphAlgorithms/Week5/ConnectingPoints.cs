using System;
using System.Collections.Generic;

namespace GraphAlgo
{
    class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Parent { get; set; }
        public int Rank { get; set; }

        public Node(int x, int y, int parent)
        {
            X = x;
            Y = y;
            Parent = parent;
            Rank = 0;
        }
    }
    class Edge
    {
        public int U { get; set; }
        public int V { get; set; }
        public double Weight { get; set; }

        public Edge(int a, int b, double c)
        {
            U = a;
            V = b;
            Weight = c;
        }
    }
    class ConnectingPoints
    {
        static double Weight(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        static int Find(int i, List<Node> nodes)
        {
            if (i != nodes[i].Parent)
            {
                nodes[i].Parent = Find(nodes[i].Parent, nodes);
            }
            return nodes[i].Parent;
        }

        static void Union(int u, int v, List<Node> nodes)
        {
            var r1 = Find(u, nodes);
            var r2 = Find(v, nodes);
            if (r1 == r2) return;
            if (nodes[r1].Rank > nodes[r2].Rank)
            {
                nodes[r2].Parent = r1;
            }
            else
            {
                nodes[r1].Parent = r2;
                if (nodes[r1].Rank == nodes[r2].Rank)
                {
                    nodes[r2].Rank++;
                }
            }
        }

        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var xList = new List<int>();
            var yList = new List<int>();

            for (var i = 0; i < n; i++)
            {
                var inputSplit = Console.ReadLine().Split();
                xList.Add(int.Parse(inputSplit[0]));
                yList.Add(int.Parse(inputSplit[1]));
            }

            Console.WriteLine(GetResult(xList, yList));
            Console.ReadKey();
        }

        private static double GetResult(List<int> x, List<int> y)
        {
            double result = 0;
            var n = x.Count;

            var nodes = new List<Node>();
            for (var i = 0; i < n; i++)
            {
                nodes.Add(new Node(x[i], y[i], i));
            }

            var edges = new List<Edge>();
            for (var i = 0; i < n; i++)
            {
                for (var j = i + 1; j < n; j++)
                {
                    edges.Add(new Edge(i, j, Weight(x[i], y[i], x[j], y[j])));
                }
            }
            edges.Sort((p, q) => p.Weight.CompareTo(q.Weight));
            for (var i = 0; i < edges.Count; i++)
            {
                var currentEdge = edges[i];
                var u = currentEdge.U;
                var v = currentEdge.V;
                if (Find(u, nodes) != Find(v, nodes))
                {
                    result += currentEdge.Weight;
                    Union(u, v, nodes);
                }
            }
            return result;
        }
    }
}
