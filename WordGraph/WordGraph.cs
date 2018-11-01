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

        private string lastAddedWord = null;

        public void AddWordToWordGraph(string word)
        {
        }

        public void FinalizeGraph()
        {
        }

        public List<List<string>> GetOrderedSequencesByLengthAndFrequency(int length, int frequency)
        {
            return null;
        }

        public List<List<string>> GetNonOrderedSequencesByLengthAndFrequency(int length, int frequency)
        {
            return null;
        }
    }
}
