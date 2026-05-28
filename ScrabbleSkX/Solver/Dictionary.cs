using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using ScrabbleSkX.Data;

namespace ScrabbleSkX.Solver
{
    public class Dictionary
    {
        //public class Node
        //{
        //    //Memory
        //    public Node Parrent;
        //    public Node[] children;
        //    public bool word;
        //    public char Character;
        //    public int Depth;

        //    //Constructor
        //    public Node(Node parrent, char character, int depth)
        //    {
        //        Parrent = parrent;
        //        children = new Node[Letters.Length];
        //        word = false;
        //        Character = character;
        //        Depth = depth;
        //    }

        //    //Methods
        //    public bool Contains(char c)
        //    {
        //        if (children[ToInt(c)] == null) return false;
        //        else return true;
        //    }
        //    public Node Child(char c)
        //    {
        //        return children[ToInt(c)];
        //    }
        //}

        public class Pattern
        {
            //Memory
            public int Length;
            public bool[] allowedLength;
            public bool[][] allowedLetter;
            public bool[] emptySquare;

            //Constructor
            public Pattern(int length)
            {
                Length = length;
                allowedLength = new bool[length + 1];
                allowedLetter = new bool[length + 1][];
                emptySquare = new bool[length + 1];

                for (int i = 1; i <= length; i++)
                {
                    allowedLength[i] = false;
                    allowedLetter[i] = new bool[Letters.Length];
                }
            }
        }

        public class Solution
        {
            //Memory
            public string Word;
            public char[] Sequence;

            //Constructor
            public Solution(string word, char[] sequence)
            {
                Word = word;
                Sequence = sequence;
            }
        }

        //Memory
        public Node root;
        string Language;
        public static char[] Letters;
        public int Count { get; set; }

        //Accessing the files


        static Dictionary<char, int> toInt = new Dictionary<char, int>();



        public Dictionary(Node root, int Count)
        {

            Language = "wwwroot\\SK";
            
            this.root = root;
            this.Count = Count;

            //Letters
            if (toInt.Count == 0)
            {
                Letters = new char[41] { 'a', 'á', 'ä', 'b', 'c', 'č', 'd', 'ď', 'e', 'é', 'f', 'g', 'h', 'i', 'í', 'j', 'k', 'l', 'ĺ', 'ľ', 'm', 'n', 'ň', 'o', 'ó', 'ô', 'p', 'r', 'ŕ', 's', 'š', 't', 'ť', 'u', 'ú', 'v', 'x', 'y', 'ý', 'z', 'ž' };
                for (int i = 0; i < Letters.Length; i++)
                    toInt.Add(Letters[i], i);
            }

            //Trie
  
        }


        //public async Task<bool> ReadSK_slovnik()
        //{

        //    Language = "wwwroot\\SK";
        //    Count = 0;

        //    //Letters
        //    if (toInt.Count == 0)
        //    {
        //        Letters = new char[41] { 'a', 'á', 'ä', 'b', 'c', 'č', 'd', 'ď', 'e', 'é', 'f', 'g', 'h', 'i', 'í', 'j', 'k', 'l', 'ĺ', 'ľ', 'm', 'n', 'ň', 'o', 'ó', 'ô', 'p', 'r', 'ŕ', 's', 'š', 't', 'ť', 'u', 'ú', 'v', 'x', 'y', 'ý', 'z', 'ž' };
        //        for (int i = 0; i < Letters.Length; i++)
        //            toInt.Add(Letters[i], i);
        //    }

        //    //Trie
        //    root = new Node(null, ' ', 0);

        //    string path = "ScrabbleWebAss.wwwroot.SK.dictionary.SK_slovnik0.txt";
        //    Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
        //    using (StreamReader sr = new StreamReader(resourceStream))
        //    {

        //        int slovnikSize = 0;
        //        while ((s = sr.ReadLine()) != null)
        //        {
        //            string[] words = s.Split(' ');
        //            foreach (string word in words)
        //                if (word.Length <= 20)
        //                {
        //                    this.Insert(word);
        //                    slovnikSize++;
        //                }
        //        }
        //        sr.Close();
        //    }
        //    return true;
        //}

        //Properties
        public int WordCount
        {
            get { return Count; }
        }

        //Methods
        public static int ToInt(char c)
        {
            return toInt[Char.ToLower(c)];
        }

        private void pInsert(string word)
        {
            Node node = root;
            for (int i = 0; i < word.Length; i++)
                if (Letters.Contains(word[i]))
                    node = Insert(word[i], node);
                else;

            //If not already present
            if (!node.word)
            {
                node.word = true;
                Count++;
            }
        }
        private Node Insert(char c, Node node)
        {
            //If present
            if (node.Contains(c)) return node.Child(c);
            //Else
            else
            {
                Node t = new Node(node, c, node.Depth + 1);
                node.children[toInt[c]] = t;
                return t;
            }
        }

        public bool Contains(string word)
        {
            Node node = root;
            for (int i = 0; i < word.Length; i++)
                if ((node = Contains(char.ToLower(word[i]), node)) == null)
                    return false;
            if (!node.word) return false;
            return true;
        }
        private Node Contains(char c, Node node)
        {
            if (node.Contains(c)) return node.Child(c);
            else return null;
        }

        public Solution[] MatchPattern(Pattern pattern, ArrayList chars)
        {
            //Found
            ArrayList found = new ArrayList();

            //Breadth-first search with paralell queues
            Queue<Node> q = new Queue<Node>();
            q.Enqueue(root);

            Queue<ArrayList> qc = new Queue<ArrayList>();
            qc.Enqueue(chars);

            Queue<ArrayList> qSequence = new Queue<ArrayList>();
            qSequence.Enqueue(new ArrayList());

            while (q.Count > 0)
            {
                Node node = q.Dequeue();
                ArrayList ch = qc.Dequeue();
                ArrayList Sequence = qSequence.Dequeue();

                if ((pattern.allowedLength[node.Depth]) && (node.word))
                {
                    //Climb up
                    Node n = node;
                    StringBuilder s = new StringBuilder();
                    do
                    {
                        s.Append(n.Character);
                        n = n.Parrent;
                    }
                    while (n.Parrent != null);

                    //Reverse
                    for (int i = 0; i < s.Length / 2; i++)
                    {
                        char c = s[i];
                        s[i] = s[s.Length - i - 1];
                        s[s.Length - i - 1] = c;
                    }
                    found.Add(new Solution(s.ToString(), (char[])Sequence.ToArray(typeof(char))));
                }

                char[] chs = (char[])(ch.ToArray(typeof(char)));

                if (node.Depth < pattern.Length)
                {
                    //Possibilities for an empty square
                    if (pattern.emptySquare[node.Depth + 1])
                    {
                        for (int i = 0; i < chs.Length; i++)
                            if ((i == 0) || (chs[i] != chs[i - 1]))
                            {
                                char C = chs[i];

                                //Joker
                                if (C == '#')
                                {
                                    for (int j = 0; j < Letters.Length; j++)
                                        if ((pattern.allowedLetter[node.Depth + 1][j]) && (node.Contains(Letters[j])))
                                        {
                                            q.Enqueue(node.Child(Letters[j]));

                                            ch.Remove(C);
                                            qc.Enqueue(new ArrayList(ch));
                                            ch.Add(C);

                                            Sequence.Add(C);
                                            qSequence.Enqueue(new ArrayList(Sequence));
                                            Sequence.RemoveAt(Sequence.Count - 1);
                                        }
                                }
                                //Regular letter
                                else
                                {
                                    if ((pattern.allowedLetter[node.Depth + 1][toInt[C]]) && (node.Contains(C)))
                                    {
                                        q.Enqueue(node.Child(C));

                                        ch.Remove(C);
                                        qc.Enqueue(new ArrayList(ch));
                                        ch.Add(C);

                                        Sequence.Add(C);
                                        qSequence.Enqueue(new ArrayList(Sequence));
                                        Sequence.RemoveAt(Sequence.Count - 1);
                                    }
                                }
                            }
                    }
                    //Possibilities for a non-empty square
                    else
                    {
                        for (int i = 0; i < Letters.Length; i++)
                            if ((pattern.allowedLetter[node.Depth + 1][i]) && (node.Contains(Letters[i])))
                            {
                                q.Enqueue(node.Child(Letters[i]));
                                qc.Enqueue(ch);
                                qSequence.Enqueue(Sequence);
                            }
                    }
                }
            }

            return (Solution[])found.ToArray(typeof(Solution));
        }

        public void Save(string path)
        {
            StreamWriter[] sw = new StreamWriter[16];
            for (int i = 2; i <= 15; i++)
                sw[i] = new StreamWriter(path + "\\" + Language + "_" + i + ".txt");

            //Breadth-first search
            Queue<Node> q = new Queue<Node>();
            q.Enqueue(root);

            while (q.Count > 0)
            {
                Node node = q.Dequeue();

                if (node.word)
                {
                    //Climb up
                    Node n = node;
                    StringBuilder s = new StringBuilder();
                    do
                    {
                        s.Append(n.Character);
                        n = n.Parrent;
                    }
                    while (n.Parrent != null);

                    //Reverse
                    for (int i = 0; i < s.Length / 2; i++)
                    {
                        char c = s[i];
                        s[i] = s[s.Length - i - 1];
                        s[s.Length - i - 1] = c;
                    }
                    sw[node.Depth].WriteLine(s);
                }

                for (int i = 0; i < Letters.Length; i++)
                    if (node.Contains(Letters[i]))
                        q.Enqueue(node.Child(Letters[i]));
            }

            for (int i = 2; i <= 15; i++)
                sw[i].Close();
        }
    }
}
