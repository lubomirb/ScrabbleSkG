using ScrabbleSkX.Data;
namespace ScrabbleSkX.Players
{
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public List<RackTile> Tiles { get; set; }
    }
}
