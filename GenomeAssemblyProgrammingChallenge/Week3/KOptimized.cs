//(Max time used: 4.49/4.50, max memory used: 40513536/536870912.)
using System;
using System.Collections.Generic;

namespace GenomeProgrammingChallenge
{
    class KOptimized
    {
        private static int n = 400;
        static void Main(string[] args)
        {
            var reads = new List<string>();
            for (var i = 0; i < n; i++)
            {
                reads.Add(Console.ReadLine());
            }

            Run(reads);
            Console.ReadKey();
        }

        static void Run(List<string> inputs)
        {
            for (var k = inputs[2].Length; k >= 1; k--)
            {
                if (IsOptimized(k, inputs))
                {
                    Console.WriteLine(k);
                    break;
                }
            }
        }

        static bool IsOptimized(int k, List<string> reads)
        {
            var kmers = new List<string>();
            foreach (var read in reads)
            {
                for (var i = 0; i < read.Length - k + 1; i++)
                {
                    kmers.Add(read.Substring(i, k));
                }
            }

            var prefixes = new List<string>();
            var suffixes = new List<string>();
            foreach (var kmer in kmers)
            {
                prefixes.Add(kmer.Substring(0, kmer.Length - 1));
                suffixes.Add(kmer.Substring(1));
            }

            return prefixes.TrueForAll(suffixes.Contains);
        }
    }
}
