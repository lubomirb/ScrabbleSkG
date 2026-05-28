using ScrabbleSkX.Data;
using ScrabbleSkX.Words;

namespace ScrabbleSkX.Players
{
    public class PlayerManager
    {
        public RackManager RackManager { get; set; }
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public Player CurrentPlayer { get; set; }
        public List<Word> PlayedWords { get; set; }

        public PlayerManager(RackManager rackManager)
        {
            RackManager = rackManager;
            PlayedWords = new List<Word>();
        }
        public void SetupPlayers()
        {
            this.PlayerOne = new Player { Name = "H1", Score = 0, Tiles = RackManager.SetupRack() };
            this.PlayerTwo = new Player { Name = "H2", Score = 0, Tiles = RackManager.SetupRack() };
            SwapCurrentPlayer();
        }
        public void SwapCurrentPlayer()
        {
            if (CurrentPlayer == null)
                CurrentPlayer = this.PlayerOne;
            else
            {
                CurrentPlayer = CurrentPlayer == PlayerOne ? PlayerTwo : this.PlayerOne;
            }
            if (CurrentPlayer == PlayerOne)
                ;//  CurrentPlayer.Tiles.ForEach(t => t.Visible = true);
        }
    }
}
