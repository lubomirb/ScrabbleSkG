using System.Reflection.Metadata;

namespace ScrabbleSkX.Data
{
    public static class Static
    {
        public const int LetterLength = 41;

        public static char[] Letters = new char[LetterLength] { 'a', 'á', 'ä', 'b', 'c', 'č', 'd', 'ď', 'e', 'é', 'f', 'g', 'h', 'i', 'í', 'j', 'k', 'l', 'ĺ', 'ľ', 'm', 'n', 'ň', 'o', 'ó', 'ô', 'p', 'r', 'ŕ', 's', 'š', 't', 'ť', 'u', 'ú', 'v', 'x', 'y', 'ý', 'z', 'ž' };
       
        public static Dictionary<char, int> toInt = new Dictionary<char, int>();

        public static void toIntSet()
        {
            for (int i = 0; i < Letters.Length; i++)
                toInt.Add(Letters[i], i);
        }
        public static int ToInt(char c)
        {
            return toInt[Char.ToLower(c)];
        }
    }
}
