using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ScrabbleSkX.Solver
{
    public class Solution : IComparable
    {
        public char dir;
        public int x;
        public int y;
        public char[] seq;
        public string s;
        public int w;
        public double probability = 1;

        //Comparer -> for the ability to sort arrays of type Solution
        int IComparable.CompareTo(object obj)
        {
            Solution s = (Solution)obj;

            //Equality
            if (this.dir == s.dir && this.x == s.x && this.y == s.y && this.seq.Length == s.seq.Length && this.s == s.s && this.w == s.w && this.probability == s.probability)
            {
                bool ok = true;
                for (int i = 0; i < seq.Length; i++)
                    if (this.seq[i] != s.seq[i])
                    {
                        ok = false;
                        break;
                    }

                if (ok)
                    return 0;
            }
            //Better
            if (this.w * this.probability > s.w * s.probability || (this.w * this.probability == s.w * s.probability && this.seq.Length > s.seq.Length))
                return -1;
            //Worse
            else
                return +1;
        }

        //'empty' solution -> for the ability to skip turns
        public static Solution EmptySolution
        {
            get
            {
                Solution es = new Solution();
                es.dir = 'H';
                es.x = 7;
                es.y = 7;
                es.seq = new char[0];
                es.s = "";
                es.w = 0;
                es.probability = 1;
                return es;
            }
        }
    }
}
