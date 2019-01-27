/* (Max time used: 0.07/1.50, max memory used: 10973184/536870912.) */
using System;
using System.Collections.Generic;

namespace AdvancedAlgos
{
    internal class AirlineCrews
    {
        private static void Main(string[] args)
        {
            var firstLine = Console.ReadLine().Split();
            var n = int.Parse(firstLine[0]);
            var m = int.Parse(firstLine[1]);

            var bipartiteGraph = new bool[n][];

            for (var i = 0; i < n; i++)
            {
                var inputs = Console.ReadLine().Split();
                if (bipartiteGraph[i] == null)
                {
                    bipartiteGraph[i] = new bool[m];
                }

                for (var j = 0; j < m; j++)
                {
                    bipartiteGraph[i][j] = inputs[j] == "1";
                }
            }

            var matching = FindMatching(bipartiteGraph);
            Console.WriteLine(string.Join(" ", matching));
            Console.ReadKey();
        }

        private static IEnumerable<int> FindMatching(IReadOnlyList<bool[]> bipartiteGraph)
        {
            var graphLength = bipartiteGraph.Count;
            var matching = new int[graphLength];
            var graph = ReadGraph(bipartiteGraph);
            MaxFlow(graph);
            for (var i = 0; i < graphLength; i++)
            {
                foreach (var id in graph.GetIds(i + 1))
                {
                    var edge = graph.GetEdge(id);
                    if (edge.Flow == 1)
                    {
                        matching[i] = edge.To - graphLength;
                        break;
                    }
                    matching[i] = -1;
                }
            }

            return matching;
        }

        private static void MaxFlow(FlowGraph graph)
        {
            while (true)
            {
                var foundPath = false;
                var queue = new Queue<int>();
                var parentIds = new int[graph.Size()];
                for (var i = 0; i < parentIds.Length; i++) parentIds[i] = -1;
                queue.Enqueue(0);
                //bfs
                while (queue.Count > 0 && !foundPath)
                {
                    var node = queue.Dequeue();
                    var ids = graph.GetIds(node);
                    foreach (var id in ids)
                    {
                        var edge = graph.GetEdge(id);
                        if (edge.Flow < edge.Capacity && parentIds[edge.To] == -1)
                        {
                            if (edge.To == edge.From) continue;
                            parentIds[edge.To] = id;
                            if (edge.To == graph.Size() - 1)
                            {
                                foundPath = true;
                                break;
                            }
                            queue.Enqueue(edge.To);
                        }
                    }
                }
                if (!foundPath) break;
                //find the value of the flow
                var to = graph.Size() - 1;
                var minCapacity = -1;
                while (to != 0)
                {
                    var id = parentIds[to];
                    var edge = graph.GetEdge(id);
                    if (minCapacity == -1 || (edge.Capacity - edge.Flow) < minCapacity) minCapacity = edge.Capacity - edge.Flow;
                    to = edge.From;
                }
                to = graph.Size() - 1;
                while (to != 0)
                {
                    var id = parentIds[to];
                    var edge = graph.GetEdge(id);
                    graph.AddFlow(id, minCapacity);
                    to = edge.From;
                }
            }
        }

        private static FlowGraph ReadGraph(IReadOnlyList<bool[]> bipartiteGraph)
        {
            var n = bipartiteGraph.Count;
            var m = bipartiteGraph[0].Length;
            var vertexCount = n + m + 2;
            var graph = new FlowGraph(vertexCount);
            for (var i = 0; i < n; i++) graph.AddEdge(0, i + 1, 1);
            for (var i = 0; i < m; i++) graph.AddEdge(n + 1 + i, n + m + 1, 1);
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < m; j++)
                {
                    if (!bipartiteGraph[i][j]) continue;
                    graph.AddEdge(i + 1, n + 1 + j, 1);
                }
            }
            return graph;
        }
    }

    internal class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Capacity { get; set; }
        public int Flow { get; set; }

        public Edge(int from, int to, int capacity)
        {
            From = from;
            To = to;
            Capacity = capacity;
            Flow = 0;
        }
    }

    internal class FlowGraph
    {
        private readonly List<Edge> _edges;
        private readonly List<int>[] _graph;

        public FlowGraph(int n)
        {
            _graph = new List<int>[n];
            for (var i = 0; i < n; ++i)
                _graph[i] = new List<int>();
            _edges = new List<Edge>();
        }

        public void AddEdge(int from, int to, int capacity)
        {
            var forwardEdge = new Edge(from, to, capacity);
            var backwardEdge = new Edge(to, from, 0);
            _graph[from].Add(_edges.Count);
            _edges.Add(forwardEdge);
            _graph[to].Add(_edges.Count);
            _edges.Add(backwardEdge);
        }

        public int Size()
        {
            return _graph.Length;
        }

        public List<int> GetIds(int from)
        {
            return _graph[from];
        }

        public Edge GetEdge(int id)
        {
            return _edges[id];
        }

        public void AddFlow(int id, int flow)
        {
            /* To get a backward edge for a true forward edge (i.e id is even), we should get id + 1
			 * due to the described above scheme. On the other hand, when we have to get a "backward"
			 * edge for a backward edge (i.e. get a forward edge for backward - id is odd), id - 1
			 * should be taken.
			 * It turns out that id ^ 1 works for both cases. Think this through! */
            _edges[id].Flow += flow;
            _edges[id ^ 1].Flow -= flow;
        }
    }
}
