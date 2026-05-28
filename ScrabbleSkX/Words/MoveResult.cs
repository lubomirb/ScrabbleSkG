
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrabbleSkX.Words;

namespace ScrabbleSkX.Words
{
    public class MoveResult
    {
        public bool Valid { get; set; }
        public List<Word> Words { get; set; }
        public int TotalScore { get; set; }
    }
}
