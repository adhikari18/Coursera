using System;
using System.Collections.Generic;
using System.Text;

namespace StringAlgoCSharp
{
    class BWT
    {
        static void Main(string[] args)
        {
            var text = Console.ReadLine();
            Console.WriteLine(GetBWT(text));
            Console.ReadKey();
        }

        static string GetBWT(string text)
        {
            var result = new StringBuilder();
            var textLength = text.Length;
            var bwtRotated = new List<string>();
            for (var i = 0; i < textLength; i++)
            {
                text = RotateString(text);
                bwtRotated.Add(text);
            }

            bwtRotated.Sort();
            foreach (var t in bwtRotated)
            {
                result.Append(t[textLength - 1]);
            }

            return result.ToString();
        }

        static string RotateString(string str)
        {
            return str[str.Length - 1] + str.Substring(0, str.Length - 1);
        }
    }
}
