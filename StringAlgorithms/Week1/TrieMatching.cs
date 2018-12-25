using System;
using System.Collections.Generic;
using System.Linq;

namespace TrieMatching
{
    public class Node
    {
        public int[] Next;

        public Node()
        {
            Next = new[]{-1, -1, -1, -1};
        }

        public bool IsLeaf()
        {
            return Next.All(i => i == -1);
        }
    }

    internal class Driver
    {
        private static void Main(string[] args)
        {
            var text = Console.ReadLine();
            var n = int.Parse(Console.ReadLine());
            var patterns = new List<string>();
            for (var i = 0; i < n; i++)
            {
                var s = Console.ReadLine();
                patterns.Add(s);
            }

            var answers = Solve(text, patterns);
            var answersLine = string.Join(" ", answers);
            Console.WriteLine(answersLine);
            Console.ReadKey();
        }

        private static List<int> Solve(string text, List<string> patterns)
        {
            var result = new List<int>();
            var trie = PatternsToTrie(patterns);
            var count = 0;
            while (!string.IsNullOrEmpty(text))
            {
                var match = PrefixTrieMatching(count++, text, trie);
                if (match != -1)
                {
                    result.Add(match);
                }
                text = text.Substring(1);
            }
            return result;
        }

        private static List<Node> PatternsToTrie(List<string> patterns)
        {
            var trie = new List<Node> {new Node()};
            foreach (var pattern in patterns)
            {
                var currentNode = trie.ElementAt(0);
                foreach (var currentSymbol in pattern)
                {
                    var index = currentNode.Next[currentSymbol.LetterToIndex()];

                    if (index != -1)
                    {
                        currentNode = trie.ElementAt(index);
                    }
                    else
                    {
                        var newNode = new Node();
                        trie.Add(newNode);
                        currentNode.Next[currentSymbol.LetterToIndex()] = trie.Count - 1;
                        currentNode = newNode;
                    }
                }
            }
            return trie;
        }

        private static int PrefixTrieMatching(int count, string text, List<Node> trie)
        {
            var currentSymbol = text[0];
            var currentNode = trie.ElementAt(0);
            var currentCharIndex = 0;
            while (true)
            {
                if (currentNode.IsLeaf())
                {
                    return count;
                }

                if (currentNode.Next[currentSymbol.LetterToIndex()] != -1)
                {
                    currentNode = trie.ElementAt(currentNode.Next[currentSymbol.LetterToIndex()]);
                    if (currentCharIndex + 1 < text.Length)
                    {
                        currentSymbol = text[++currentCharIndex];
                    }
                    else
                    {
                        if (currentNode.IsLeaf())
                        {
                            return count;
                        }
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            return -1;
        }


    }

    public static class Extensions
    {
        public static int LetterToIndex(this char letter)
        {
            switch (letter)
            {
                case 'A':
                    return 0;
                case 'C':
                    return 1;
                case 'G':
                    return 2;
                case 'T':
                    return 3;
                default:
                    return -1;
            }
        }
    }


}
