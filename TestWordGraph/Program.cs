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
                using (StreamReader sr = new StreamReader(@"C: \Users\Karim\Desktop\UWdatascience\C#\SeqFinder\TestWordGraph\ML.txt"))
                {

                    String line = sr.ReadToEnd();

                    for (int i = 0; i < line.Length; i++)
                    {
                        char c = line[i];

                        if (char.IsPunctuation(c))
                        {
                            line = line.Remove(i, 1);
                            i--;
                        }
                    }

                    words = line.Split(' ');
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            wordGraph.SetLastAddedWord(words.GetValue(0));

            for (int i = 1; i < words.Length; i++)
            {
                wordGraph.AddWordToWordGraph((String)words.GetValue(i));
            }

            for(int i = 2; i < words.Length; i++)
            {
                wordGraph.inputToOutputEdges(words.GetValue(i - 2), words.GetValue(i - 1), words.GetValue(i));
            }

            // wordGraph.PrintWordGraph();

            while (true)
            {
                int length = 0;
                int frequency = 0;

                Console.WriteLine("Please input length and then press enter");
                int.TryParse(Console.ReadLine(), out length);

                Console.WriteLine("Please input frequency and then press enter");
                int.TryParse(Console.ReadLine(), out frequency);

                List<List<string>> print = wordGraph.GetOrderedSequencesByLengthAndFrequency(length, frequency);
                wordGraph.printSequences(print);

                Console.WriteLine("Please press any key if you would like to try another combination");
                Console.ReadKey();
                Console.WriteLine();
            }

        }

    }
}

