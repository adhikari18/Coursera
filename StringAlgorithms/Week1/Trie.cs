using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgos
{
    class Driver
    {
        static void Main(string[] args)
        {
            
            int n = int.Parse(Console.ReadLine());
            List<string> patterns = new List<string>();
            for (int i = 0; i < n; i++)
            {
                string s = Console.ReadLine();
                patterns.Add(s);
            }
            var trieObj = new Trie();
            var trie = trieObj.BuildTrie(patterns);

            for (int i = 0; i < trie.Count(); i++)
            {
                foreach (var edge in trie[i])
                {
                    Console.WriteLine("{0}->{1}:{2}", i, edge.Value.ToString(), edge.Key);
                }
            }

            Console.ReadKey();
        }
    }

    class Trie
    {
        public List<Dictionary<char, int>> BuildTrie(List<string> patterns)
        {
            var trie = new List<Dictionary<char, int>>();
            var root = new Dictionary<char, int>();
            trie.Add(root);

            foreach (var pattern in patterns)
            {
                var currentNode = root;
                for (int i = 0; i < pattern.ToCharArray().Length; i++)
                {
                    var currentSymbol = pattern[i];

                    if (currentNode.ContainsKey(currentSymbol))
                    {
                        currentNode = trie.ElementAt(currentNode[currentSymbol]);
                    }
                    else
                    {
                        var newNode = new Dictionary<char, int>();
                        trie.Add(newNode);
                        currentNode.Add(currentSymbol, trie.Count - 1);
                        currentNode = newNode;
                    }
                }
            }
            return trie;
        }
    }
}
