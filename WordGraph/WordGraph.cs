using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordAnalysis
{
    public class WordGraph
    {
        private class Node
        {
            public string word = null;
            public int count = 0;
            public Dictionary<string, Edge> edges = null;
        }

        private class Edge
        {
            public Node fromNode = null;
            public Node toNode = null;
            public int weight = 0;
        }

        private Dictionary<string, Node> wordGraph = new Dictionary<string, Node>();

        private Node lastAddedWord = null;

        public void SetLastAddedWord(Object firstWord)
        {
            Node n = new Node();
            n.word = (String) firstWord;
            n.count = 1;
            n.edges = new Dictionary<string, Edge>();

            lastAddedWord = n;
        }

        public void AddWordToWordGraph(string word)
        {

            Node node = new Node();
            node.word = word;
            node.count = 1;
            node.edges = new Dictionary<string, Edge>();

            if (!lastAddedWord.edges.ContainsKey(word))
            {
                Edge edge = new Edge();
                edge.fromNode = lastAddedWord;
                edge.toNode = node;
                edge.weight = 1;

                lastAddedWord.edges.Add(word, edge);
            }
            else
            {
                Edge edge = null;
                lastAddedWord.edges.TryGetValue(word, out edge);

                edge.weight = edge.weight + 1;
            }
            if (!wordGraph.ContainsKey(word))
            {
                wordGraph.Add(word, node);
            } else
            {
                wordGraph.TryGetValue(word, out node);
                node.count = node.count + 1;
            }

            lastAddedWord = node;
   
        }

        public void FinalizeGraph()
        {
        }

        public void PrintWordGraph()
        {
            for(int i = 0; i < wordGraph.Count; i++)
            {
                Console.WriteLine("Unique word " + wordGraph.ElementAt(i).Key + " number " + i + " has occured " + wordGraph.ElementAt(i).Value.count + " times and is connected to " + wordGraph.ElementAt(i).Value.edges.Count + "words");
                
                for(int j = 0; j < wordGraph.ElementAt(i).Value.edges.Count; j++)
                {
                    Console.WriteLine(" word " + j + ": " + wordGraph.ElementAt(i).Value.edges.ElementAt(j).Value.toNode.word + " conencted: " + wordGraph.ElementAt(i).Value.edges.ElementAt(j).Value.weight + " times");
                }
                
                Console.ReadKey();
            }
        }

        private List<List<Node>> frequencyArray = new List<List<Node>>();

        public void createFrequencyArray()
        {
            frequencyArray.Add(new List<Node>());

            for(int i = 0; i < wordGraph.Count; i++)
            {
                int count = wordGraph.ElementAt(i).Value.count;

                if(count > frequencyArray.Count)
                {
                    for(int j = 0; j < (frequencyArray.Count - count); j++)
                    {
                        frequencyArray.Add(new List<Node>());
                    }
                }

                frequencyArray.ElementAt(count).Add(wordGraph.ElementAt(i).Value);
            }
        }

        public List<string> RecursiveSeqSearch(int length, Node node)
        {



            return null;
        }

        public List<List<string>> GetOrderedSequencesByLengthAndFrequency(int length, int frequency)
        {
            if(frequencyArray.Count < frequency)
            {
                return null;
            }

            List<List<string>> orderedSequences = new List<List<string>>();

            createFrequencyArray();
            
            for(int i = frequency; i < frequencyArray.Count; i++)
            {
                for(int j = 0; j < frequencyArray.ElementAt(i).Count; j++)
                {
                   orderedSequences.Add(RecursiveSeqSearch(length, frequencyArray.ElementAt(i).ElementAt(j)));
                }
            }


            return orderedSequences;
        }

        public List<List<string>> GetNonOrderedSequencesByLengthAndFrequency(int length, int frequency)
        {
            return null;
        }
    }
}
