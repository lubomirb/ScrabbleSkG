using ScrabbleSkX.Data;

namespace ScrabbleSkX.Words
{
    public class Word : IEquatable<Word>
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public int Score { get; set; }
        public string Text { get; set; }
        public List<BoardTile> Tiles { get; set; }
        public bool Valid { get; set; }

        public bool Equals(Word other)
        {
            if (other is null)
                return false;

            return StartX == other.StartX &&
                   StartY == other.StartY &&
                   EndX == other.EndX &&
                   EndY == other.EndY &&
                   Text == other.Text;
        }

        public override bool Equals(object obj) => Equals(obj as Word);

        public override int GetHashCode()
        {
            return HashCode.Combine(StartX, StartY, EndX, EndY, Text);
        }
        public void SetValidHighlight()
        {
            foreach (var t in Tiles)
                t.OnHighlight(Valid);
        }

        public void ClearValidHighlight()
        {
            foreach (var t in Tiles)
                t.ClearHighlight();
        }
    }

}
