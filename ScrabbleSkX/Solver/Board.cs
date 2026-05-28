
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScrabbleSkX.Solver
{
    public class Board
    {
        //Memory
        BackgroundWorker bgWorker;

        Dictionary dc { get; set; }
        Rack R { get; set; }

        ArrayList solutions;
        BagTiles bagTiles = new BagTiles();

        //Paralell arrays
        Letter[,] Content = new Letter[15, 15];
        int[,] xW = new int[15, 15];
        int[,] xL = new int[15, 15];
        char[,][] Possibilities = new char[15, 15][];
        int[,] Values = new int[15, 15];
        int[,] minLength = new int[15, 15];
        int[,] maxLength = new int[15, 15];

        //Properties
        public int maxL = 7;
        public int SolutionsCount = 100;

        public Board(Dictionary solverDictionary, string boardIni)
        {
            Content = new Letter[15, 15];

            char[] delimtC = { '\r', '\n' };
            string delimtS = "\r\n";
           
            if (string.IsNullOrWhiteSpace(boardIni))
                throw new ArgumentException("board.ini je prázdny alebo sa nepodarilo načítať.");

            string[] lines = boardIni.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length < 32)
                throw new ArgumentException("board.ini má nesprávny formát."+
                    " Skontrolujte či sú v súbore 32 riadkov (15 pre xW, prázdny riadok, 15 pre xL).");


            string s = lines[0];

            for (int i = 0; i < 15; i++)
            {
                s = lines[i + 1];
                for (int j = 0; j < 15; j++)
                    xW[i, j] = int.Parse(s[j].ToString());
            }

            for (int i = 0; i < 15; i++)
            {
                s = lines[i + 17];
                for (int j = 0; j < 15; j++)
                    xL[i, j] = int.Parse(s[j].ToString());
            }


            // dc = new Dictionary();

            dc = solverDictionary;
        }

        public Board(Board source, Solution solution = null)
        {
            dc = source.dc;
            maxL = source.maxL;
            SolutionsCount = source.SolutionsCount;

            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                {
                    Content[i, j] = source.Content[i, j];
                    xW[i, j] = source.xW[i, j];
                    xL[i, j] = source.xL[i, j];
                }

            //If a solution is provided, apply it
            if (solution != null)
            {
                //Place it on the board
                int i = solution.x;
                int j = solution.y;

                foreach (char character in solution.seq)
                {
                    //Find insert position
                    if (solution.dir == 'H')
                        while (Content[i, j] != null) j++;
                    else if (solution.dir == 'V')
                        while (Content[i, j] != null) i++;

                    //Letter
                    char letter = character;
                    int worth = bagTiles.GetLetterValue(letter);

                    if (character == '#')
                        letter = char.ToUpper(solution.s[(i - solution.x) + (j - solution.y)]);
                    Content[i, j] = new Letter(letter, worth);
                }
            }
        }

        //Properties
        public Letter this[int i, int j]
        {
            get { return Content[i, j]; }
            set { Content[i, j] = value; }
        }
        public Dictionary Dictionary
        {
            set { dc = value; }
        }

        //Public methods
        public Color getColor(int i, int j)
        {
            if (xW[i, j] == 1 && xL[i, j] == 1)
                return Color.FromName("LightSeaGreen");

            switch (xW[i, j])
            {
                case 2: return Color.Pink;
                case 3: return Color.Red;
            }
            switch (xL[i, j])
            {
                case 2: return Color.LightBlue;
                case 3: return Color.Blue;
            }

            //Should not happen
            return Color.Black;
        }

        public void findMoves(BackgroundWorker backgroundWorker, Rack rack)
        {
            bgWorker = backgroundWorker;
            R = rack;

            solutions = new ArrayList();
            solutions.Clear();

            //Vertically
            setLengths();
            generate_possibilities();
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    startWordAt(i, j, 'V');
                    if (bgWorker.CancellationPending)
                    {
                        return;
                    }
                }
                bgWorker.ReportProgress((int)((double)(50 * i) / (double)15), BestSolutions());
            }

            //Horizontally
            rotateBoard();
            setLengths();
            generate_possibilities();
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    startWordAt(i, j, 'H');
                    if (bgWorker.CancellationPending)
                    {
                        rotateBoard();
                        return;
                    }
                }
                bgWorker.ReportProgress(50 + (int)((double)(50 * i) / (double)15), BestSolutions());
            }
            rotateBoard();
        }

        public Solution[] findMoves(Rack rack)
        {
            R = rack;


            solutions = new ArrayList();
            solutions.Clear();

            //Vertically
            setLengths();
            generate_possibilities();
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    startWordAt(i, j, 'V');

            //Horizontally
            rotateBoard();
            setLengths();
            generate_possibilities();
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    startWordAt(i, j, 'H');
            rotateBoard();

            return BestSolutions();
        }

        private void setLengths()
        {
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                {
                    //maxLength
                    int k = i;
                    int movesRemaining = maxL;

                    while (k < 15 && (movesRemaining > 0 || Content[k, j] != null))
                    {
                        if (Content[k, j] == null)
                            movesRemaining--;
                        k++;
                    }
                    maxLength[i, j] = k - i;

                    //minLength
                    k = i;
                    while ((k < 15) && (!neighbourhood(k, j))) k++;

                    if (k == 15) minLength[i, j] = 15 + 1;
                    else minLength[i, j] = Math.Max(k - i + 1, 2);
                }
        }

        private void generate_possibilities()
        {
            //For all squares
            for (int X = 0; X < 15; X++)
                for (int Y = 0; Y < 15; Y++)
                    if (Content[X, Y] != null)
                        Possibilities[X, Y] = new char[] { Content[X, Y].character };
                    else
                    {
                        Values[X, Y] = 0;

                        //Has vertical neighbours
                        if (((Y > 0) && (Content[X, Y - 1] != null)) || ((Y < 14) && (Content[X, Y + 1] != null)))
                        {
                            //Construct the string
                            StringBuilder s = new StringBuilder();

                            int a = Y;
                            int b = Y;
                            while ((a >= 1) && (Content[X, a - 1] != null)) a--;
                            while ((b < 14) && (Content[X, b + 1] != null)) b++;

                            for (int i = a; i <= b; i++)
                                if (i == Y)
                                    s.Append(' ');
                                else
                                {
                                    Values[X, Y] += Content[X, i].worth;
                                    s.Append(Content[X, i].character);
                                }

                            //Check possible chars
                            ArrayList possible_chars = new ArrayList();

                            foreach (char C in Dictionary.Letters)
                            {
                                s[Y - a] = C;
                                if (dc.Contains(s.ToString()))
                                    possible_chars.Add(C);
                            }

                            Possibilities[X, Y] = (char[])possible_chars.ToArray(typeof(char));
                        }
                        //No vertical neighbours
                        else
                            Possibilities[X, Y] = Dictionary.Letters;
                    }
        }

        private void rotateBoard()
        {
            for (int i = 0; i < 15; i++)
                for (int j = i + 1; j < 15; j++)
                {
                    Letter t = Content[i, j];
                    Content[i, j] = Content[j, i];
                    Content[j, i] = t;
                }
        }

        private void startWordAt(int X, int Y, char direction)
        {
            //Console.WriteLine("Starting word at [" + X + " ; " + Y + "]");

            //Only if previous is empty or does not exist
            if ((X > 0) && (Content[X - 1, Y] != null)) return;

            //Only if minLength is not bigger than maxLength
            if (minLength[X, Y] > maxLength[X, Y]) return;

            //Generate pattern
            Dictionary.Pattern pattern = new Dictionary.Pattern(maxLength[X, Y]);
            for (int i = 0; i < maxLength[X, Y]; i++)
            {
                if ((i + 1 >= minLength[X, Y]) && ((X + i + 1 == 15) || (Content[X + i + 1, Y] == null)))
                    pattern.allowedLength[i + 1] = true;

                for (int j = 0; j < Possibilities[X + i, Y].Length; j++)
                    pattern.allowedLetter[i + 1][Dictionary.ToInt(Possibilities[X + i, Y][j])] = true;

                if (Content[X + i, Y] == null)
                    pattern.emptySquare[i + 1] = true;
                else
                    pattern.emptySquare[i + 1] = false;
            }

            //Generate array of chars available
            ArrayList characters = new ArrayList();
            for (int i = 0; i < R.Length; i++)
                if (R[i] != null)
                    characters.Add(R[i].character);
            characters.Sort();

            //Get sequences of chars
            Dictionary.Solution[] Solutions = dc.MatchPattern(pattern, characters);

            foreach (Dictionary.Solution Solution in Solutions)
            {
                //Sequence of chars to be inserted, including '#' for joker
                char[] chars = Solution.Sequence;
                string word = Solution.Word;

                //Get the value
                int value = 0;
                int multiply = 1;
                int vertical_values = 0;

                int addingCharacter = 0;
                for (int i = 0; i < word.Length; i++)
                    if (Content[X + i, Y] != null)
                        value += Content[X + i, Y].worth;
                    else
                    {
                        //Main word
                        multiply *= xW[X + i, Y];
                        value += bagTiles.GetLetterValue(chars[addingCharacter]) * xL[X + i, Y];

                        //Vertical word
                        if (vertical_neighbourhood(X + i, Y))
                            vertical_values += (Values[X + i, Y] + bagTiles.GetLetterValue(chars[addingCharacter]) * xL[X + i, Y]) * xW[X + i, Y];

                        addingCharacter++;
                    }
                value *= multiply;
                value += vertical_values;
                //Bingo
                if (chars.Length == 7) value += 50;

                //Add solution
                Solution sol = new Solution();
                sol.dir = direction;
                if (direction == 'H')
                {
                    sol.x = Y;
                    sol.y = X;
                }
                else
                {
                    sol.x = X;
                    sol.y = Y;
                }
                sol.seq = chars;
                sol.s = word;
                sol.w = value;

                double probability = 1;
                if (R.Length > 777)
                    for (int k = 0; k < sol.seq.Length; k++)
                        probability *= (double)(7 - k) * (double)R.countLetter(sol.seq[k]) / (double)(R.Length - k);
                sol.probability = probability;

                solutions.Add(sol);
            }
        }

        private bool neighbourhood(int i, int j)
        {
            if (Content[i, j] != null) return false;
            if ((i - 1 >= 0) && (Content[i - 1, j] != null)) return true;
            if ((i + 1 < 15) && (Content[i + 1, j] != null)) return true;
            if ((j - 1 >= 0) && (Content[i, j - 1] != null)) return true;
            if ((j + 1 < 15) && (Content[i, j + 1] != null)) return true;
            if ((i == 7) && (j == 7)) return true;
            return false;
        }

        private bool vertical_neighbourhood(int i, int j)
        {
            if (Content[i, j] != null) return false;
            if ((j - 1 >= 0) && (Content[i, j - 1] != null)) return true;
            if ((j + 1 < 15) && (Content[i, j + 1] != null)) return true;
            return false;
        }

        private Solution[] BestSolutions()
        {
            int N = Math.Min(solutions.Count, SolutionsCount);

            //Stable sort
            Solution[] Solutions = (Solution[])solutions.ToArray(typeof(Solution));
            ArrayList newSolutions = new ArrayList();

            for (int i = 0; i < N; i++)
            {
                for (int j = i + 1; j < solutions.Count; j++)
                    if (Solutions[i].w * Solutions[i].probability < Solutions[j].w * Solutions[j].probability)
                    {
                        Solution t = Solutions[i];
                        Solutions[i] = Solutions[j];
                        Solutions[j] = t;
                    }
                newSolutions.Add(Solutions[i]);
            }
            //solutions.Sort();
            //solutions.RemoveRange(N, solutions.Count - N);
            solutions = newSolutions;

            return (Solution[])solutions.ToArray(typeof(Solution));
        }
    }
}