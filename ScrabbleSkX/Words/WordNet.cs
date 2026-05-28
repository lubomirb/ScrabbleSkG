using ScrabbleSkX.Data;

namespace ScrabbleSkX.Words
{
    public class Word
    {

        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public int Score { get; set; }
        public string Text { get; set; }
        public List<BoardTile> Tiles { get; set; }
        public bool Valid { get; set; }

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

        public override string ToString()
        {
            return $"<Word Text={Text}, SX={StartX}, EX={EndX}, SY={StartY}, EY={EndY}, Score={Score} />";
        }
    }

}
