using System.Collections;

namespace ScrabbleSkX.Solver
{
    public class Bag
    {
        string Language = "SK";

        char[] letters;
        Letter[] Content;

        static Dictionary<char, int> LetterValues;
        Dictionary<char, int> LetterCounts;

       // Dictionary<char, int> LetterWorths;

        public object HostEnvironment { get; private set; }

        public Bag(string skIni)
        {
            
       
            ArrayList alContent = new ArrayList();
            ArrayList alLetters = new ArrayList();
            LetterValues = new Dictionary<char, int>();
            LetterCounts = new Dictionary<char, int>();
            
            char[] delimtC = { '\r', '\n' };
            string[] delimtS = { "\r\n", "\n" , "\r" };

           
            string[] lines = skIni.Split(delimtS, StringSplitOptions.None);
           
                    char[] separators = new char[2] { ' ', ' ' };
                    string[] parts = new string[3];

                    foreach( string line in lines)
                    {
                        parts = line.Split(separators);
                        char Character = parts[0][0];
                        int Count = int.Parse(parts[1]);
                        int Worth = int.Parse(parts[2]);

                        alLetters.Add(Character);
                        LetterValues.Add(Character, Worth);
                        LetterCounts.Add(Character, Count);


                        for (int i = 0; i < Count; i++)
                            alContent.Add(new Letter(Character, Worth));
                    }
     

            Content = (Letter[])alContent.ToArray(typeof(Letter));
            letters = (char[])alLetters.ToArray(typeof(char));
        }
        public static int GetLetterValue(char letter)
        {
            return LetterValues[letter];
        }
    }
}
