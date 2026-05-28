namespace ScrabbleSkX.Solver
{
    public class BagTiles
    {
        private Dictionary<char, (int count, int value)>? tileDistribution;

        public BagTiles()
        {
            tileDistribution = new Dictionary<char, (int count, int value)>
            {
                { 'a', (9, 1)},
                { 'á', (1, 4)},
                { 'ä', (1, 10)},
                { 'b', (2, 4)},
                { 'c', (1, 4)},
                { 'č', (1, 5)},
                { 'd', (3, 2)},
                { 'ď', (1, 8)},
                { 'e', (8, 1)},
                { 'é', (1, 7)},
                { 'f', (1, 8)},
                { 'g', (1, 8)},
                { 'h', (1, 4)},
                { 'i', (5, 1)},
                { 'í', (1, 5)},
                { 'j', (2, 3)},
                { 'k', (3, 2)},
                { 'l', (3, 2)},
                { 'ľ', (1, 7)},
                { 'ĺ', (1, 10)},
                { 'm', (4, 2)},
                { 'n', (5, 1)},
                { 'ň', (1, 8)},
                { 'o', (9, 1)},
                { 'ó', (1, 10)},
                { 'ô', (1, 8)},
                { 'p', (3, 2)},
                { 'r', (4, 1)},
                { 'ŕ', (1, 10)},
                { 's', (4, 1)},
                { 'š', (1, 5)},
                { 't', (4, 1)},
                { 'ť', (1, 7)},
                { 'u', (2, 3)},
                { 'ú', (1, 7)},
                { 'v', (4, 1)},
                { 'x', (1, 10)},
                { 'y', (1, 4)},
                { 'ý', (1, 5)},
                { 'z', (1, 4)},
                { 'ž', (1, 5)},
                { '#', (2, 0)} // žolíky
            };
        }

        public int GetLetterValue(char letter)
        {
            return tileDistribution![letter].value;
        }
    }
}
