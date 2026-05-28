using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrabbleSkX.Data
{
    public class BagTile
    {
        public List<char> Letters { get; set; }
        public string JokerBag { get; set; }

        public List<char> AbecedaSK = new List<char> 
        {
            'A', 'Á', 'Ä', 'B', 'C', 'Č', 'D', 'Ď', 'E', 'É', 'F',
            'G', 'H', 'I', 'Í', 'J', 'K', 'L', 'Ĺ', 'Ľ', 'M', 'N',
            'Ň', 'O', 'Ó', 'Ô', 'P', 'R', 'Ŕ', 'S', 'Š', 'T', 'Ť',
            'U', 'Ú', 'V', 'X', 'Y', 'Ý', 'Z', 'Ž', '#' 
        };
        /// <summary>
        /// 'a', 'á', 'ä', 'b', 'c', 'č', 'd', 'ď', 'e', 'é', 'f',
        /// 'g', 'h', 'i', 'í', 'j', 'k', 'l', 'ĺ', 'ľ', 'm', 'n',
        /// 'ň', 'o', 'ó', 'ô', 'p', 'r', 'ŕ', 's', 'š', 't', 'ť',
        /// 'u', 'ú', 'v', 'x', 'y', 'ý', 'z', 'ž', '#'
        /// </summary>
        /// 


        public BagTile()
        {
            //SetupBag();
            SetupBagSK();
        
        }

        /// <summary>
        /// Sets up a tile bag ready for a game.
        /// </summary>
        public void SetupBag()
        {
            Letters = new List<char>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                for (int x = 0; x < LetterCount(c); x++)
                {
                    this.Letters.Add(c);
                }
            }

            Letters = Letters.OrderBy(l => Guid.NewGuid()).ToList();
        }
        public void SetupBagSK()
        {
            
            Letters = new List<char>();
            foreach (char c in AbecedaSK)
            {
                for (int x = 0; x < LetterCount(c); x++)
                {
                    this.Letters.Add(c);
                }
            }

            Letters = Letters.OrderBy(l => Guid.NewGuid()).ToList();
            JokerBag = Letters.FirstOrDefault().ToString(); 
        }

        /// <summary>
        /// Returns how many letters are left in the rack.
        /// </summary>
        public int LetterCountRemaining()
        {
            return Letters.Count;
        }

        public List<char> LettersRemaining()
        {
            return Letters;
        }


        /// <summary>
        /// Give a letter back to the bag. This would be triggered when a user swaps a tile.
        /// </summary>
        /// <param name="letter"></param>
        public void GiveLetter(char letter)
        {
            Letters.Add(letter);
        }

        public string TakeLetters(int numLetters)
        {
            var letters = string.Empty;
            var random = new Random();

            while (letters.Length < numLetters)
            {
                // Ran out of letters
                if (Letters.Count == 0)
                    break;

                var randomLetter = Letters[random.Next(0, Letters.Count)];
                letters += randomLetter;
                Letters.Remove(randomLetter);
            }

            return letters;
        }

        /// <summary>
        /// How many times times a provided character appear in the tile bag
        /// at the start of a game?
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int LetterCount(char c)
        {
            var timesMapping = new Dictionary<char, int>()
            {
                {'A', 9 }, {'Á', 1 }, {'Ä', 1 }, {'B', 2 }, {'C', 1 }, {'Č', 1 }, {'D', 3 }, {'Ď', 1 }, {'E', 8 }, {'É', 1 }, {'F', 1 },
                {'G', 1 }, {'H', 1 }, {'I', 5 }, {'Í', 1 }, {'J', 2 }, {'K', 3 }, {'L', 3 }, {'Ĺ', 1 }, {'Ľ', 1 }, {'M', 4 }, {'N', 5 },
                {'Ň', 1 }, {'O', 9 }, {'Ó', 1 }, {'Ô', 1 }, {'P', 3 }, {'R', 4 }, {'Ŕ', 1 }, {'S', 4 }, {'Š', 1 }, {'T', 4 }, {'Ť', 1 },
                {'U', 2 }, {'Ú', 1 }, {'V', 4 }, {'X', 1 }, {'Y', 1 }, {'Ý', 1 }, {'Z', 1 }, {'Ž', 1 }, {'#', 2}
            //    { 'E', 13 }, { 'A', 9 }, { 'I', 8 }, { 'O', 8 },
            //    { 'N', 5 }, { 'R', 6 }, { 'T', 7 }, { 'L', 4  },
            //    { 'S', 5 }, { 'U', 4 }, { 'D', 5 }, { 'G', 3 },
            //    { 'B', 2 }, { 'C', 2 }, { 'M', 2 }, { 'P', 2 },
            //    { 'F', 2 }, { 'H', 4 }, { 'V', 2 }, { 'W', 2 },
            //    { 'Y', 2 }, { 'K', 1 }, { 'J', 1 }, { 'X', 1 },
            //    { 'Q', 1 }, { 'Z', 1 },
            };

            return timesMapping[c];
        }
    }
}
