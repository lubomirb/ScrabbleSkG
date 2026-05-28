namespace ScrabbleSkX.Data
{
    public class Node
    {
        //Memory
        public Node Parrent;
        public Node[] children;
        public bool word;
        public char Character;
        public int Depth;

        //Constructor
        public Node(Node parrent, char character, int depth)
        {
            Parrent = parrent;
            children = new Node[Static.LetterLength];
            word = false;
            Character = character;
            Depth = depth;
        }
        public bool Contains(char c)
        {
            if (children[Static.ToInt(c)] == null) return false;
            else return true;
        }
        public Node Child(char c)
        {
            return children[Static.ToInt(c)];
        }
    }
}
