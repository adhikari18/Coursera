//Assembling the phi X174 Genome from Error-Free Reads Using Overlap Graphs
//(Max time used: 2.28/4.50, max memory used: 19095552/536870912.)
using System;
using System.Collections.Generic;

namespace GenomeAssembly
{
    public class Vertex
    {
        public int VertexNum; 
        public string Read;
        public Dictionary<int, int> Edges;
        public bool Found;

        public Vertex(int vertexNum, string read)
        {
            VertexNum = vertexNum;
            Read = read;
            Edges = new Dictionary<int, int>();
            Found = false;
        }
    }

    class GenomeAssembly
    {
        private static int _last = -1;
        private static void MakeOverlapGraph(Vertex[] graph)
        {
            for (var i = 0; i < graph.Length; i++)
            {
                for (var j = 0; j < graph.Length; j++)
                {
                    if (j == i)
                    {
                        continue;
                    }
                    var str1 = graph[i].Read.ToCharArray();
                    var str2 = graph[j].Read.ToCharArray();
                    for (var k = 0; k < str2.Length; k++)
                    {
                        var length = str1.Length - k;
                        var error = (int)(length * 0.03);
                        var overlap = true;
                        var m = 0;
                        for (var l = k; l < str1.Length; l++)
                        {
                            if (error == 0)
                            {
                                overlap = false;
                                break;
                            }
                            if (str1[l] != str2[m])
                            {
                                error--;
                            }
                            m++;
                        }

                        if (overlap)
                        {
                            graph[i].Edges.Add(j, length);
                            break;
                        }
                    }
                }
            }
        }

        private static string GetResult(Vertex[] graph)
        {
            var genome = string.Empty;
            var random = new Random().Next(graph.Length);
            var first = random;

            genome = genome + graph[random].Read;
            genome = Explore(graph, random, genome);

            for (var i = 0; i < graph.Length; i++)
            {
                if (graph[i].Found) continue;
                genome = genome + graph[i].Read;
                _last = i;
            }

            if (graph[_last].Edges.ContainsKey(first))
            {
                int length;
                graph[_last].Edges.TryGetValue(first, out length);
                return genome.Substring(length);
            }

            return genome;
        }
        
        private static string Explore(Vertex[] graph, int random, string genome)
        {
            graph[random].Found = true;
            var max = -1;
            var index = -1;
            foreach (var key in  graph[random].Edges.Keys)
            {
                int temp;
                 graph[random].Edges.TryGetValue(key, out temp);
                 if (graph[key].Found || max >= temp) continue;
                 max = temp;
                index = key;
            }

            if (index == -1)
            {
                return genome;
            }

            genome = genome + graph[index].Read.Substring(max);
            _last = index;
            return Explore(graph, index, genome);
        }

        static void Main(string[] args)
        {
            var graph = GetReads();  
            MakeOverlapGraph(graph);    
            Console.WriteLine(GetResult(graph));
            Console.ReadKey();

        }
        public static Vertex[] GetReads()
        {
            var graph = new Vertex[1618];

            for (var i = 0; i < graph.Length; i++)
            {
                graph[i] = new Vertex(i, Console.ReadLine());
            }
            return graph;
        }
    }
}
