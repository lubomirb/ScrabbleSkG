using ScrabbleSkX.Words;

namespace ScrabbleSkX.Data
{
    public class RackManager
    {
        public const int RACK_WIDTH = 7;
        public BagTile BagTile { get; set; }
        public List<RackTile> SetupRack()
        {
            var tiles = new List<RackTile>();
            for (int i = 0; i < RACK_WIDTH; i++)
            {
                var tile = new RackTile();
                tiles.Add(tile);
            }
            FillRack(tiles);
            return tiles;
        }
        public RackManager(BagTile bagTile)
        {
            BagTile = bagTile;
        }

        public bool IsRackEmpty(List<RackTile> tiles)
        {
            int i = tiles.Where(r => string.IsNullOrEmpty(r.Text)).Count();
            return i == RACK_WIDTH;
        }

        public bool FillRack(List<RackTile> tiles)
        {
             int missingLetters = tiles.Where(r => string.IsNullOrEmpty(r.Text)).Count();
            var letters = BagTile.TakeLetters(missingLetters);

            for (int i = 0; i < letters.Length; i++)
            {
                var tile = tiles.FirstOrDefault(r => string.IsNullOrEmpty(r.Text));
                tile.Letter = letters.Substring(i, 1);
                tile.LetterValue = WordScorer.LetterValue(Char.Parse(letters.Substring(i, 1)));
                tile.Text = tile.Letter;
            }
 
            return letters.Length == 7; 
        }
      
        public void ClearRack(List<RackTile> tiles, string solution)
        {
            for (int i = 0; i < solution.Length; i++)
            {
                var tile = tiles.FirstOrDefault(t => t.Text == solution.Substring(i, 1));
                tile.ClearDisplay();
            }

        }
    }
}
