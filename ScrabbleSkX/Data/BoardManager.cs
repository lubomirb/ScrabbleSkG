
using System.Drawing;
using ScrabbleSkX.Words;
using ScrabbleSkX.Stats;
using ScrabbleSkX.Players;
using ScrabbleSkX.Solver;
//using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ScrabbleSkX.Data
{
    public class BoardManager
    {
        public const int BOARD_WIDTH = 15;
        public const int BOARD_HEIGHT = 15;

        public BagTile bagTile { get; set; }

        public StatManager statManager { get; set; }
        public PlayerManager playerManager { get; set; }

        public BoardTile[,] BoardTiles;

        private readonly ILogger<BoardManager> _logger;
        public BoardManager(ILogger<BoardManager> logger) { _logger = logger; }

        public BoardManager(BagTile pbagTile, StatManager pstatManager, PlayerManager pplayerManager)
        {
            bagTile = pbagTile;
            statManager = pstatManager;
            playerManager = pplayerManager;
        }
        public void SetupTiles(string initialBoard)
        {
            BoardTiles = new BoardTile[BOARD_WIDTH, BOARD_HEIGHT];

            var specialTilePositions = WordScorer.GetTileTypes(initialBoard);

            for (int x = 1; x <= BOARD_WIDTH; x++)
            {
                for (int y = 1; y <= BOARD_HEIGHT; y++)
                {
                    var tile = new BoardTile
                    {
                        BagTile = bagTile,
                        XLoc = x - 1,
                        YLoc = y - 1,
                        Text = string.Empty,
                        Letter = string.Empty,
                        LetterValue = 0,
                        BoardTileType = specialTilePositions[x - 1, y - 1],
                        TileInPlay = false,
                        MatejPlayed = false,
                        Joker = false
                    };

                    tile.SetRegularBackgroundColour();

                    BoardTiles[x - 1, y - 1] = tile;
                }
            }
        }

        /// <summary>
        /// Ensure that where the tiles have been placed on the board are valid locations.
        /// </summary>
        /// <returns></returns>
        public bool ValidateTilePositions()
        {
            var tilesInPlay = new List<BoardTile>();

            if (statManager.Moves == 0)
            {
                // On the first move the centre tile needs to be in play.
                if (!BoardTiles[(int)BoardManager.BOARD_WIDTH / 2, (int)BoardManager.BOARD_HEIGHT / 2].TileInPlay)
                {
                    // MessageBox.Show("One of your letters must be on the centre tile for the first move.");
                    return false;
                }
            }
            else
            {
                // Evey move other than the first one, at least one of your tiles needs to be touching an existing tile.
                if (!ValidateATileIsAdjacent())
                {
                    // MessageBox.Show("Invalid letter positioning. At least one of your tiles must be adjacent to existing letters");
                    return false;
                }
            }

            // Grab all the tiles in play in this turn.
            for (int x = 0; x < BoardManager.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < BoardManager.BOARD_HEIGHT; y++)
                {
                    if (BoardTiles[x, y].TileInPlay)
                        tilesInPlay.Add(BoardTiles[x, y]);
                }
            }

            var direction = GetMovementDirection();

            // Only one tile in play and/or we aren't moving in any direction so the move should be valid.
            if (direction == MovementDirection.None || tilesInPlay.Count() <= 1)
                return true;

            // For every tile in play, ensure the player hasn't tried to move in two directions at once.
            for (int x = 1; x < tilesInPlay.Count; x++)
            {
                int xChange = tilesInPlay[x - 1].XLoc - tilesInPlay[x].XLoc;
                int yChange = tilesInPlay[x - 1].YLoc - tilesInPlay[x].YLoc;

                if (direction == MovementDirection.Across)
                {
                    if (yChange != 0)
                        return false;
                }

                if (direction == MovementDirection.Down)
                {
                    if (xChange != 0)
                        return false;
                }
            }

            int xLoc = tilesInPlay[0].XLoc;
            int yLoc = tilesInPlay[0].YLoc;
            int lastX = tilesInPlay[tilesInPlay.Count() - 1].XLoc;
            int lastY = tilesInPlay[tilesInPlay.Count() - 1].YLoc;

            // Ensure that there are no gaps inbetween the tile placements even in the same direction
            if (direction == MovementDirection.Across)
            {
                for (int x = xLoc; x <= lastX; x++)
                {
                    if (!BoardTiles[x, yLoc].TileInPlay && string.IsNullOrEmpty(BoardTiles[x, yLoc].Text))
                        return false;
                }
            }
            if (direction == MovementDirection.Down)
            {
                for (int y = yLoc; y <= lastY; y++)
                {
                    if (!BoardTiles[xLoc, y].TileInPlay && string.IsNullOrEmpty(BoardTiles[xLoc, y].Text))
                        return false;
                }
            }

            return true;
        }
        /// <summary>
        /// For all the tiles in play, validate that at least one is adjacent to an existing tile on the board.
        /// </summary>
        /// <returns></returns>
        public bool ValidateATileIsAdjacent()
        {
            var adjacentTiles = new List<BoardTile>();

            for (int x = 0; x < BoardManager.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < BoardManager.BOARD_HEIGHT; y++)
                {
                    if (BoardTiles[x, y].TileInPlay)
                    {
                        if (x > 0)
                            adjacentTiles.Add(BoardTiles[x - 1, y]);

                        if (x < BoardManager.BOARD_WIDTH - 1)
                            adjacentTiles.Add(BoardTiles[x + 1, y]);

                        if (y > 0)
                            adjacentTiles.Add(BoardTiles[x, y - 1]);

                        if (y < BoardManager.BOARD_HEIGHT - 1)
                            adjacentTiles.Add(BoardTiles[x, y + 1]);
                    }
                }
            }

            return adjacentTiles.Any(t => !t.TileInPlay && !string.IsNullOrEmpty(t.Text));
        }

        /// <summary>
        /// Return the direction the player is moving on the board with their tiles. 
        /// Either across, down or none.
        /// </summary>
        /// <returns></returns>
        public MovementDirection GetMovementDirection()
        {
            var tilesInPlay = new List<BoardTile>();

            for (int x = 0; x < BoardManager.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < BoardManager.BOARD_HEIGHT; y++)
                {
                    if (BoardTiles[x, y].TileInPlay)
                        tilesInPlay.Add(BoardTiles[x, y]);
                }
            }

            // No direction because less than 2 tiles have been played
            if (tilesInPlay.Count <= 1)
                return MovementDirection.None;

            int xChange = tilesInPlay[1].XLoc - tilesInPlay[0].XLoc;
            int yChange = tilesInPlay[1].YLoc - tilesInPlay[0].YLoc;

            return xChange > 0 ? MovementDirection.Across : yChange > 0 ? MovementDirection.Down : MovementDirection.None;
        }

        /// <summary>
        /// Reset the tiles which are flagged as 'in play' after a players turn.
        /// </summary>
        public void ResetTilesInPlay()
        {
            for (int x = 0; x < BoardManager.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < BoardManager.BOARD_HEIGHT; y++)
                {
                    BoardTiles[x, y].TileInPlay = false;
                    BoardTiles[x, y].Joker = false;
                    BoardTiles[x, y].SetRegularBackgroundColour();
                }
            }

            ClearTileHighlights();
        }

        /// <summary>
        /// Reset any tile highlighting on the board
        /// </summary>
        public void ClearTileHighlights()
        {
            for (int x = 0; x < BoardManager.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < BoardManager.BOARD_HEIGHT; y++)
                {
                    BoardTiles[x, y].ClearHighlight();
                }
            }

            // ScrabbleForm.btnPlay.Text = "Play";
        }


    }
}
