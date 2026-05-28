using System.Reflection;
using ScrabbleSkX.Data;

namespace ScrabbleSkX.Words
{

    public static class WordScorer
    {
        private static BoardTileType[,] _tilePositions;

        /// <summary>
        /// Get the total score for a word. Use's the words location on the board to appropriately apply
        /// word modifiers such as Double Word, Triple Letter etc...
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static int ScoreWord(Word word)
        {
            string initialBoard = "";
            if (_tilePositions == null)
                LoadTilePositions(initialBoard);

            var score = 0;
            int krat = 1;

            foreach (var tile in word.Tiles)
            {
                var tileScore = 0;
                //tileScore = LetterValue(tile.Letter[0]);
                tileScore = (int)tile.LetterValue;

                // Only "in play" tiles can be affected by the tile modifier.

                if (tile.TileInPlay)
                    switch (tile.BoardTileType)
                    {
                        case BoardTileType.TripleLetter:
                            if (!tile.Joker)
                                tileScore *= 3;
                            break;
                        case BoardTileType.TripleWord:
                            krat *= 3;
                            break;
                        case BoardTileType.DoubleLetter:
                            if (!tile.Joker)
                                tileScore *= 2;
                            break;
                        case BoardTileType.DoubleWord:
                            krat *= 2;
                            break;
                        case BoardTileType.Center:
                            krat *= 2;
                            break;
                        default:
                            break;
                    }

                score += tileScore;
            }
            return score * krat;
        }

        /// <summary>
        /// Get the "raw" score of a word, i.e that is just the score of the letters in the word,
        /// this will not use the position of the board to apply triple/double modifiers etc.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static int RawWordScore(string word)
        {
            return word.Sum(c => LetterValue(c));
        }

        /// <summary>
        /// Get the locations of the different types of tiles on the board.
        /// </summary>
        /// <returns></returns>
        public static BoardTileType[,] GetTileTypes(string initialBoard)
        {
            if (_tilePositions == null)
                LoadTilePositions(initialBoard);

            return _tilePositions;
        }

        /// <summary>
        /// Parse the input file for the locations of the special tiles on the board such as triple word, double letter etc.
        /// </summary>

        private static void LoadTilePositions(string initialBoard)
        {
            _tilePositions = new BoardTileType[15, 15];


            int row = 0;
            int col = 0;

            if (string.IsNullOrWhiteSpace(initialBoard))
                throw new ArgumentException("initial_board.txt je prázdny alebo sa nepodarilo načítať.");


            string[] lines = initialBoard.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 15)
                throw new ArgumentException("initial_board.txt má nesprávny formát.");

            foreach (string line in lines)
            {
                foreach (var tp in line.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(p => p.Trim())
                                        .ToArray())
                {
                    if (string.IsNullOrEmpty(tp))
                        continue;

                    switch (tp.Trim())
                    {
                        case "RE":
                            _tilePositions[row, col] = BoardTileType.Regular;
                            break;
                        case "CE":
                            _tilePositions[row, col] = BoardTileType.Center;
                            break;
                        case "TW":
                            _tilePositions[row, col] = BoardTileType.TripleWord;
                            break;
                        case "TL":
                            _tilePositions[row, col] = BoardTileType.TripleLetter;
                            break;
                        case "DW":
                            _tilePositions[row, col] = BoardTileType.DoubleWord;
                            break;
                        case "DL":
                            _tilePositions[row, col] = BoardTileType.DoubleLetter;
                            break;
                        default:
                            throw new Exception($"Unknown tile type in inital_board file: {tp}");
                    }
                    col += 1;
                }

                col = 0;
                row += 1;
            }

        }

        /// <summary>
        /// How many points is a provided character worth?
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int LetterValue(char c)
        {
            var scoreMapping = new Dictionary<char, int>()
            {
                {'A', 1 }, {'Á', 4 }, {'Ä', 10 }, {'B', 4 }, {'C', 4 }, {'Č', 5 }, {'D', 2 }, {'Ď', 8 }, {'E', 1 }, {'É', 7 }, {'F', 8 },
                {'G', 8 }, {'H', 4 }, {'I', 1 }, {'Í', 5}, {'J', 3 }, {'K', 2 }, {'L', 2 }, {'Ĺ', 10 }, {'Ľ', 7 }, {'M', 2 }, {'N', 1 },
                {'Ň', 8 }, {'O', 1 }, {'Ó', 10 }, {'Ô', 8 }, {'P', 2 }, {'R', 1 }, {'Ŕ', 10 }, {'S', 1 }, {'Š', 5}, {'T', 1 }, {'Ť', 7 },
                {'U', 3 }, {'Ú', 7 }, {'V', 1 }, {'X', 10 }, {'Y', 4 }, {'Ý', 5 }, {'Z', 4 }, {'Ž', 5}, {'#', 0}
                // One point
//            //    { 'E', 1 }, { 'A', 1 }, { 'I', 1 }, { 'O', 1 },
//                { 'R', 1 }, { 'T', 1 }, { 'S', 1 },
//
                // Two points
//                { 'D', 2 }, { 'U', 2 }, { 'L', 2 }, { 'N', 2 },

                // Three points
//               { 'G', 3 },  { 'Y', 3 }, { 'H', 3 },

                // Four points
//                { 'F', 4 }, { 'W', 4 }, { 'B', 4 },
//               { 'C', 4 }, { 'M', 4 }, { 'P', 4 }, 

                // Five points
//                { 'K', 5 }, { 'V', 5 },

                // Eight points
//                { 'X', 8 },

               // Ten pints
//               { 'Q', 10 }, { 'Z', 10 }, { 'J', 10 }
            };

            return scoreMapping[c];
        }
    }

}
