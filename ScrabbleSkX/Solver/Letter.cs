using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace ScrabbleSkX.Solver
{
    public class Letter
    {
        //Memory
        char Character;
        int Worth;

        //Constructor
        public Letter(char character, int worth)
        {
            Character = character;
            Worth = worth;
        }

        //Properties
        public char character
        {
            get { return Character; }
            set { Character = value; }
        }
        public int worth
        {
            get { return Worth; }
            set { Worth = value; }
        }
    }
}
