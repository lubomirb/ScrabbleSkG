using ScrabbleSkX.Data;
using ScrabbleSkX.Players;
using ScrabbleSkX.Solver;
using ScrabbleSkX.Stats;
using ScrabbleSkX.GameLog;
using ScrabbleSkX.Words;

namespace ScrabbleSkX.Services
{
    // Simple application-wide state container to preserve game state across navigations
    public class GameState
    {
        public bool Initialized { get; set; } = false;

        public GameLog.GameLog? GameLog { get; set; }
        public Data.Trie? Trie { get; set; }
        public Solver.Dictionary? SolverDictionary { get; set; }

        public bool GamePlaying { get; set; }
        public bool SwapIsClicked { get; set; }
        public string? PlayMessage { get; set; }

        public StatManager? StatManager { get; set; }
        public BagTile? TileBag { get; set; }
        public RackManager? RackManager { get; set; }
        public PlayerManager? PlayerManager { get; set; }

        public string? InitialBoard { get; set; }
        public BoardManager? BoardManager { get; set; }
        public string? BoardIni { get; set; }
        public Solver.Board? SolverBoard { get; set; }

        public Words.WordValidator? WordValidator { get; set; }
    }
}
