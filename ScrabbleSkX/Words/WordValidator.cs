//using Scrabble.Core.Movement;
using ScrabbleSkX;
using ScrabbleSkX.Data;
using ScrabbleSkX.Players;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ScrabbleSkX.Words
{
    public class WordValidator
    {
        public List<string> ValidWords { get; set; }

        public BagTile BagTile { get; set; }

        public PlayerManager playerManager { get; set; }
        public BoardManager boardManager { get; set; }
        public ScrabbleSkX.Solver.Dictionary solverDictionary { get; set; }
        public HashSet<string> validWords = new HashSet<string>();
        public HashSet<string> invalidWords = new HashSet<string>();

        public WordValidator(BagTile pBagTile, PlayerManager pplayerManager, BoardManager pboardManager, ScrabbleSkX.Solver.Dictionary psolverDictionary)
        {
            BagTile = pBagTile;
            boardManager = pboardManager;
            playerManager = pplayerManager;
            solverDictionary = psolverDictionary;
            //this.LoadWords();
        }


        /// <summary>
        /// Check if a provided word is present in the list of known valid words.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool CheckWord(Word word)
        {

            return solverDictionary.Contains(word.Text) || validWords.Contains(word.Text);

        }

        /// <summary>
        /// Validate all the words on the board.
        /// </summary>
        /// <returns></returns>
        public MoveResult ValidateAllWordsInPlay()
        {
            var words = new List<Word>();
            int totalScore = 0;
            int missingLetters = playerManager.CurrentPlayer.Tiles.Where(r => string.IsNullOrEmpty(r.Text)).Count();

            for (int x = 0; x < BoardManager.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < BoardManager.BOARD_HEIGHT; y++)
                {
                    if (!string.IsNullOrEmpty(boardManager.BoardTiles[x, y].Text) && boardManager.BoardTiles[x, y].TileInPlay)
                    {
                        foreach (var w in GetSurroundingWords(x, y))
                        {
                            // Todo: need to allow duplicated words if the word actually has been played twice
                            // Think this is sorted, just need to test it.

                            if (!words.Contains(w) && !playerManager.PlayedWords.Contains(w))
                                words.Add(w);
                        }
                    }
                }
            }

            foreach (var w in words)
            {
                w.Tiles = GetWordTiles(w);
                w.Score = WordScorer.ScoreWord(w);
                w.Valid = CheckWord(w);
                w.SetValidHighlight();
            }


            return new MoveResult
            {
                TotalScore = words.Sum(w => w.Score) + totalScore,
                Words = words,
                Valid = words.All(w => w.Valid)
            };

        }
        public List<Word> GetInvalidWords()
        {
            var words = new List<Word>();
            var invalidwords = new List<Word>();

            //int missingLetters = ScrabbleForm.PlayerManager.CurrentPlayer.Tiles.Where(r => string.IsNullOrEmpty(r.Text)).Count();

            for (int x = 0; x < BoardManager.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < BoardManager.BOARD_HEIGHT; y++)
                {
                    if (!string.IsNullOrEmpty(boardManager.BoardTiles[x, y].Text) && boardManager.BoardTiles[x, y].TileInPlay)
                    {

                        foreach (var w in GetSurroundingWords(x, y))
                        {
                            // Todo: need to allow duplicated words if the word actually has been played twice
                            // Think this is sorted, just need to test it.

                            if (!words.Contains(w) && !boardManager.playerManager.PlayedWords.Contains(w))
                                words.Add(w);
                        }

                    }
                }
            }

            foreach (var w in words)
            {
                w.Tiles = GetWordTiles(w);
                w.Score = WordScorer.ScoreWord(w);
                w.Valid = CheckWord(w);
                if (!w.Valid)
                    invalidwords.Add(w);
                w.SetValidHighlight();
            }
            return invalidwords;
        }
        /// <summary>
        /// Get the tiles from the game board that a word has been played on.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public List<BoardTile> GetWordTiles(Word word)
        {
            var tiles = new List<BoardTile>();

            if (word.StartX != word.EndX)
            {
                // Word is played horizontally
                for (var x = word.StartX; x <= word.EndX; x++)
                    tiles.Add(boardManager.BoardTiles[x, word.StartY]);
            }
            else
            {
                // Word is played vertically
                for (var y = word.StartY; y <= word.EndY; y++)
                    tiles.Add(boardManager.BoardTiles[word.StartX, y]);
            }

            return tiles;
        }


        /// <summary>
        /// Traverse the board horizontally and vertically from a given point (x, y)
        /// to find the full word in play in both the horizontal and vertical direction.
        /// These words are then validated to ensure that the move is valid.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public List<Word> GetSurroundingWords(int x, int y)
        {
            var words = new List<Word>();

            string horizontal = string.Empty;
            string vertical = string.Empty;

            // Start/End location for the horizonal word
            var tx = x;
            while (tx >= 0 && !string.IsNullOrEmpty(boardManager.BoardTiles[tx, y].Text))
                tx -= 1;
            tx += 1;

            var tx2 = x;
            while (tx2 < BoardManager.BOARD_WIDTH && !string.IsNullOrEmpty(boardManager.BoardTiles[tx2, y].Text))
                tx2 += 1;
            tx2 -= 1;

            for (var i = Math.Max(tx, 0); i <= Math.Min(tx2, BoardManager.BOARD_WIDTH - 1); i++)
                horizontal += boardManager.BoardTiles[i, y].Text;

            // Start/End location for the vertical word
            var ty = y;
            while (ty >= 0 && !string.IsNullOrEmpty(boardManager.BoardTiles[x, ty].Text))
                ty -= 1;
            ty += 1;

            var ty2 = y;
            while (ty2 < BoardManager.BOARD_WIDTH && !string.IsNullOrEmpty(boardManager.BoardTiles[x, ty2].Text))
                ty2 += 1;
            ty2 -= 1;

            for (var i = Math.Max(ty, 0); i <= Math.Min(ty2, BoardManager.BOARD_HEIGHT - 1); i++)
                vertical += boardManager.BoardTiles[x, i].Text;

            if (!string.IsNullOrEmpty(horizontal) && horizontal.Length > 1)
                words.Add(new Word
                {
                    StartX = tx,
                    EndX = tx2,
                    StartY = y,
                    EndY = y,
                    Text = horizontal
                });

            if (!string.IsNullOrEmpty(vertical) && vertical.Length > 1)
                words.Add(new Word
                {
                    StartX = x,
                    EndX = x,
                    StartY = ty,
                    EndY = ty2,
                    Text = vertical
                });

            return words;
        }
        public void Valid()
        {
            List<Word> wordList = GetInvalidWords();
            foreach (var w in wordList)
                validWords.Add(w.Text);
        }
        public void InValid(string word)
        {
            invalidWords.Add(word);
        }
        public bool CheckInvalid(string word)
        {
            return invalidWords.Contains(word);
        }
        public void SaveInValidWords()
        {

            string filename = "ScrabbleSK_ValidWords.txt";
            FileStream fileStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 80, FileOptions.WriteThrough);
            StreamWriter streamLog = new StreamWriter(fileStream);

            foreach (var word in validWords)
            {
                streamLog.WriteLine(word);
            }
            streamLog.Close();

            filename = "ScrabbleSK_InValidWords.txt";
            using (TextWriter writer = File.AppendText(filename))
            {
                foreach (var word in invalidWords)
                    writer.WriteLine(word);
            }


        }
    }
}
