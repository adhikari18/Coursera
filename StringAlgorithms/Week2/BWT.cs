using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            StringBuilder result = new StringBuilder();

            var bwtRotation = new List<string>();
            var tempString = text;
            for (var i = 0; i < text.Length; i++)
            {
                tempString = RotateString(tempString);
                bwtRotation.Add(tempString);
            }

            bwtRotation.Sort();
            for (var i = 0; i < text.Length; i++)
            {
                result.Append(bwtRotation[i][text.Length - 1]);
            }

            return result.ToString();
        }

        static string RotateString(string str)
        {
            if (str.Length <= 1) return str;
            return str[str.Length - 1] + str.Substring(0, str.Length - 1);
        }
    }
}
