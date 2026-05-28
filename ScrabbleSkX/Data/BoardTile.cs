using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Web;


namespace ScrabbleSkX.Data
{

    public class BoardTile
    {
        public BagTile BagTile { get; set; }
        public int XLoc { get; set; }
        public int YLoc { get; set; }
  
        public string Text { get; set; }
        public string Letter { get; set; }
         public int? LetterValue { get; set; }
        
        public bool TileInPlay { get; set; }
        public bool MatejPlayed { get; set; }

        public BoardTileType BoardTileType { get; set; }
        public string TileClass { get; set; }
        public string TileTypeMsg { get; set; }
        public bool Joker { get; set; }

        public void OnLetterPlaced(string letter, int? letterValue)
        {
            this.Letter = letter;
            this.Text = letter;
            this.LetterValue = letterValue;
            if (letter == "#")
            {
                this.Text = BagTile.JokerBag;
                this.Joker = true;
            }
            this.TileInPlay = true;
            SetRegularBackgroundColour();
        }

        public void OnLetterRemoved()
        {
            this.Letter = string.Empty;
            this.Text = string.Empty;
            this.TileInPlay = false;
            SetRegularBackgroundColour();
        }

        public void OnHighlight(bool valid)
        {
            //this.FlatStyle = FlatStyle.Flat;
            //this.FlatAppearance.BorderColor = valid ? Color.LimeGreen : Color.DarkRed;
            //this.FlatAppearance.BorderSize = 5;
        }

        public void ClearHighlight()
        {
            //this.FlatStyle = FlatStyle.Standard;
        }

        public void SetRegularBackgroundColour()
        {
            switch (this.BoardTileType)
            {
                case BoardTileType.Regular:
                    this.TileClass = "regular-letter";
                    this.TileTypeMsg = "";
                    break;
                case BoardTileType.Center:
                    this.TileClass = "center-letter";
                    this.TileTypeMsg = "2xw";
                    break;
                case BoardTileType.TripleWord:
                    this.TileClass = "tripple-word";
                    this.TileTypeMsg = "3xw";
                    break;
                case BoardTileType.TripleLetter:
                    this.TileClass = "tripple-letter";
                    this.TileTypeMsg = "3xl";
                    break;
                case BoardTileType.DoubleWord:
                    this.TileClass = "double-word";
                    this.TileTypeMsg = "2xw";
                    break;
                case BoardTileType.DoubleLetter:
                    this.TileClass = "double-letter";
                    this.TileTypeMsg = "2xl";
                    break;
                default:
                    this.TileClass = string.Empty;
                    break;
            }

            // if (!string.IsNullOrEmpty(this.Letter))
            //    this.TileClass = "not-empty";

            if (this.TileInPlay)
                this.TileClass = "inplay-letter";
        }

         public string TileSpaceClass()
        {
            string c = "tile-space";
            if (string.IsNullOrEmpty(this.Letter))
            {
                switch (this.BoardTileType)
                {
                    case BoardTileType.Regular:
                        c += " regular-letter";

                        break;
                    case BoardTileType.Center:
                        c += " center-letter";

                        break;
                    case BoardTileType.TripleWord:
                        c += " tripple-word";

                        break;
                    case BoardTileType.TripleLetter:
                        c += " tripple-letter";

                        break;
                    case BoardTileType.DoubleWord:
                        c += " double-word";

                        break;
                    case BoardTileType.DoubleLetter:
                        c += " double-letter";

                        break;
                    default:
                        ;
                        break;
                }

            }
            return c;
        }
    }
}

