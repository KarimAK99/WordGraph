using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WordAnalysis;

namespace TestWordGraph
{
    class Program
    {

        private static WordGraph wordGraph = new WordGraph();
        private static string[] words;

        static void Main(string[] args)
        {
            try
            {
                using (StreamReader sr = new StreamReader("ML.txt"))
                {

                    String line = sr.ReadToEnd();
                    words = line.Split(' ');

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            for (int i = 0; i < words.Length; i++)
            {
                wordGraph.AddWordToWordGraph((String)words.GetValue(i));
            }
        }

    }
}

