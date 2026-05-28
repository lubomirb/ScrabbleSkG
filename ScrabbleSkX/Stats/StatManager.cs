using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrabbleSkX.Stats
{
    public class StatManager
    {
        public int Moves { get; set; }

        public int Passes { get; set; }

        public int Swaps { get; set; }

        // Todo: need to set these and present them somewhere (menu or end of game??)
        public int HighestScoringWordScore { get; set; }

        public string HighestScoringWord { get; set; }

        public int ConsecutivePasses { get; set; }
        public StatManager()
        {
            Moves = 0; 
            Passes = 0; 
            Swaps = 0; 
            HighestScoringWordScore = 0;
            HighestScoringWord = string.Empty;
            ConsecutivePasses = 0;
        }

    }
}
