namespace ScrabbleSkX.Data
{
    public class Trie
    {
        public Node root { get; set; }

        public int Count { get; set; }

        //Methods

        public Trie()
        {
            root = new Node(null, ' ', 0);
        }

        public void Insert(string word)
        {
            Node node = root;
            for (int i = 0; i < word.Length; i++)
                if (Static.Letters.Contains(word[i]))
                    node = Insert(word[i], node);
                else;

            //If not already present
            if (!node.word)
            {
                node.word = true;
                Count++;
            }
        }
        public Node Insert(char c, Node node)
        {
            //If present
            if (node.Contains(c)) return node.Child(c);
            //Else
            else
            {
                Node t = new Node(node, c, node.Depth + 1);
                node.children[Static.toInt[c]] = t;
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
    }
}
