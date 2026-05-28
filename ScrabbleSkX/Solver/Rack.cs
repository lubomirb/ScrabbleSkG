using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrabbleSkX.Solver
{
    public class Rack
    {
        //Memory
        Letter[] Content;
        Letter l = new Letter('a', 0);

        //Constructor
        public Rack()
        {
            Content = new Letter[7];
            for (int i = 0; i < 7; i++)
                Content[i] = null;
        }
        public Rack(int size)
        {
            Content = new Letter[size];
            for (int i = 0; i < size; i++)
                Content[i] = null;
        }
        public Rack(List<char> tileRack)
        {
            char c;
            Content = new Letter[7];
            for (int i = 0; i < tileRack.Count; i++)
            {
                c = tileRack[i];
                if (tileRack[i] != '#')
                    c = char.ToLower(tileRack[i]);

                Content[i] =
                    new Letter(c, 0);
            }
        }
        public Rack(Rack source, Solution solution)
        {
            //source
            Content = new Letter[source.Length];
            for (int i = 0; i < source.Length; i++)
                Content[i] = source.Content[i];

            //solution
            foreach (char c in solution.seq)
                this.GetAndRemoveLetter(c);
        }

        //Properties
        public int Length
        {
            get { return Content.Length; }
        }
        public int Population
        {
            get
            {
                int c = 0;
                for (int i = 0; i < Content.Length; i++)
                    if (Content[i] != null)
                        c++;
                return c;
            }
        }

        public Letter this[int i]
        {
            get { return Content[i]; }
            set { Content[i] = value; }
        }

        //Public methods
        public Letter GetAndRemoveLetter(char letter)
        {
            //Find the character (including #)
            for (int i = 0; i < Content.Length; i++)
                if (Content[i] != null && Content[i].character == letter)
                {
                    Letter toReturn = Content[i];
                    Content[i] = null;
                    return toReturn;
                }

            //Not found
            return null;
        }
        public int countLetter(char letter)
        {
            int c = 0;
            for (int i = 0; i < Content.Length; i++)
                if (Content[i] != null && Content[i].character == letter)
                    c++;
            return c;
        }

    }
}
