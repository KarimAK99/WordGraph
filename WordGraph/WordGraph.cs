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
            public List<Wegw> ratios = new List<Wegw>();
            public List<int> positions = null;
            public Tree sequences = null;
            public int location = 0;
        }

        private class Edge
        {
            public Node fromNode = null;
            public Node toNode = null;
            public int weight = 0;
        }

        private class Wegw
        {
            public Node fromNode = null;
            public Node middleNode = null;
            public Node toNode = null;
            public int weight = 0;
        }

        private class Tree
        {
            public TNode root = null;
            public List<TNode> allNodes = null;
            public int minFrequency = 0;
            public int depth = 1;
        }

        private class TNode
        {
            public Node node = null;
            public TNode parent = null;
            public List<TNode> children = null;
            public List<int> indices;
            public int depth = 2;
            
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

        private List<string> text = new List<string>();
        private List<List<int>> allPositions = new List<List<int>>();

        public void AddWordToWordGraph(string word)
        {
            text.Add(word);

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
                node.positions = new List<int>();
                node.positions.Add(text.Count);
                wordGraph.Add(word, node);
                node.location = wordGraph.Count - 1;
            } else
            {
                wordGraph.TryGetValue(word, out node);
                node.count = node.count + 1;
                node.positions.Add(text.Count);
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
                
               // Console.ReadKey();
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
                    for(int j = frequencyArray.Count; j < count; j++)
                    {
                        frequencyArray.Add(new List<Node>());
                    }
                }

                frequencyArray.ElementAt(count - 1).Add(wordGraph.ElementAt(i).Value);
            }
        }

        public void inputToOutputEdges(Object fN, Object mN, Object tN)
        {
            Node fromNode;
            wordGraph.TryGetValue((String) fN, out fromNode);

            Node middleNode;
            wordGraph.TryGetValue((String) mN, out middleNode);

            Node toNode;
            wordGraph.TryGetValue((String) tN, out toNode);

            for (int i = 0; i < middleNode.ratios.Count; i++)
            {
                Boolean notFound = true;

                if(middleNode.ratios.ElementAt(i).fromNode == fromNode && middleNode.ratios.ElementAt(i).toNode == toNode)
                {
                    middleNode.ratios.ElementAt(i).weight = middleNode.ratios.ElementAt(i).weight + 1;
                    notFound = false;
                }
                if (notFound)
                {
                    Wegw connection = new Wegw();
                    connection.fromNode = fromNode;
                    connection.middleNode = middleNode;
                    connection.toNode = toNode;
                    connection.weight = 1;

                    middleNode.ratios.Add(connection);
                }
            }
                
        }
        
        public void CreateAllPositions()
        {
            for(int i = 0; i < wordGraph.Count; i++)
            {
                allPositions.Add(new List<int>());
                
                for(int j = 0; j < wordGraph.ElementAt(i).Value.positions.Count; j++)
                {
                    allPositions.ElementAt(i).Add(wordGraph.ElementAt(i).Value.positions.ElementAt(j));
                }
            }
        }

        public List<int> compareIndices(List<int> first, List<int> second)
        {
            int firstIndex = 0;
            int secondIndex = 0;
            List<int> indices = new List<int>();

            while (firstIndex < first.Count && secondIndex < second.Count)
            {
                if(first.ElementAt(firstIndex) > second.ElementAt(secondIndex))
                {
                    secondIndex++;
                } else
                {
                    if(first.ElementAt(firstIndex) + 1 == second.ElementAt(secondIndex))
                    {
                        indices.Add(secondIndex);
                        firstIndex++;
                        secondIndex++;

                    } else
                    {
                        firstIndex++;
                    }
                }
            }

            return indices;
        }

        public void createTree(Object inputnode)
        {
            Node node = (Node) inputnode;
            Tree sequences = new Tree();
            TNode root = new TNode();
            root.node = node;
            root.depth = 1;
            sequences.allNodes = new List<TNode>();
            sequences.allNodes.Add(root);
            createChildren(node, root, sequences);
            sequences.depth = 2;
            node.sequences = sequences;
        }

        public void createChildren(Object inputnode, Object parentTnode, Object inputtree)
        {
            Node node = (Node) inputnode;
            TNode parent = (TNode) parentTnode;
            Tree tree = (Tree) inputtree;

            for (int i = 0; i < node.edges.Count; i++)
            {
                TNode child = new TNode();
                child.parent = parent;
                child.node = new Node();
                wordGraph.TryGetValue(node.edges.ElementAt(i).Key, out child.node);
                child.indices = new List<int>();
             //   Console.WriteLine("first " + node.location + " second " + child.node.location);
              //  Console.WriteLine("size " + allPositions.Count);
                child.indices = compareIndices(allPositions.ElementAt(node.location), allPositions.ElementAt(child.node.location));
                child.depth = parent.depth + 1;
              //  Console.WriteLine("depth " + child.depth);
                tree.allNodes.Add(child);
    
            }
        }

        public void increaseDepth(Object rootTree)
        {
            Tree tree = (Tree) rootTree;
           
            for(int i = 0; i < tree.allNodes.Count; i++)
            {
                if(tree.allNodes.ElementAt(i).depth == tree.depth)
                {
                    createChildren(tree.allNodes.ElementAt(i).node, tree.allNodes.ElementAt(i), tree);
                }
            }

            tree.depth++;
        }

        public List<List<string>> GetOrderedSequencesByLengthAndFrequency(int length, int frequency)
        {
       //     if(frequencyArray.Count < frequency)
        //    {
        //        return null;
         //   }

            List<List<string>> orderedSequences = new List<List<string>>();

            CreateAllPositions();
            createFrequencyArray();
            
            for(int i = frequency; i < frequencyArray.Count; i++)
            {
                for(int j = 0; j < frequencyArray.ElementAt(i).Count; j++)
                {
                  for(int k = 0; k < frequencyArray.ElementAt(i).ElementAt(j).edges.Count; k++)
                    {
                        if (frequencyArray.ElementAt(i).ElementAt(j).edges.ElementAt(k).Value.fromNode.sequences == null)
                        {
                            createTree(frequencyArray.ElementAt(i).ElementAt(j).edges.ElementAt(k).Value.fromNode);
                        }
                        
                        for(int x = frequencyArray.ElementAt(i).ElementAt(j).edges.ElementAt(k).Value.fromNode.sequences.depth; x < length; x++)
                        {
                            increaseDepth(frequencyArray.ElementAt(i).ElementAt(j).edges.ElementAt(k).Value.fromNode.sequences);
                        }

                        for(int y = 0; y < frequencyArray.ElementAt(i).ElementAt(j).edges.ElementAt(k).Value.fromNode.sequences.allNodes.Count; y++)
                        {
                            if (frequencyArray.ElementAt(i).ElementAt(j).edges.ElementAt(k).Value.fromNode.sequences.allNodes.ElementAt(y).indices != null)
                            {
                                if (frequencyArray.ElementAt(i).ElementAt(j).edges.ElementAt(k).Value.fromNode.sequences.allNodes.ElementAt(y).depth == length)
                                {
                                    if (frequencyArray.ElementAt(i).ElementAt(j).edges.ElementAt(k).Value.fromNode.sequences.allNodes.ElementAt(y).indices.Count == frequency)
                                    {

                                        orderedSequences.Add(new List<string>());
                                        int last = frequencyArray.ElementAt(i).ElementAt(j).edges.ElementAt(k).Value.fromNode.sequences.allNodes.ElementAt(y).indices.Count - 1;
                                        int position = frequencyArray.ElementAt(i).ElementAt(j).edges.ElementAt(k).Value.fromNode.sequences.allNodes.ElementAt(y).indices.ElementAt(last);

                                    //    Console.WriteLine(text.Count);

                                        for (int z = position - length; z < position; z++)
                                        {
                                      //      Console.WriteLine(" z: " + z);
                                            orderedSequences.ElementAt(orderedSequences.Count - 1).Add(text.ElementAt(z));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return orderedSequences;
        }

        public List<List<string>> GetNonOrderedSequencesByLengthAndFrequency(int length, int frequency)
        {
            return null;
        }

        public void printSequences(List<List<string>> sequences)
        {
            if (sequences == null)
            {
                Console.WriteLine("No sequences found");
            }
            else
            {
                for (int i = 0; i < sequences.Count; i++)
                {
                    Console.Write("Sequence number " + i + ": ");

                    for (int j = 0; j < sequences.ElementAt(i).Count; j++)
                    {
                        Console.Write(sequences.ElementAt(i).ElementAt(j) + " ");
                    }

                    Console.WriteLine();
                }
            }


        }
    }
}
