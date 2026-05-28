using Microsoft.Extensions.Logging;
using System.Drawing;

namespace ScrabbleSkX.Data
{
    public class RackTile
    {
        public string Letter { get; set; }
        public string Text { get; set; }

        public int? LetterValue { get; set; }
        public bool LetterSelected { get; set; }

        public void ClearDisplay()
        {
            //ILogger.LogInformation($"BoardTileClick0 {t.Letter} {t.Text}");
            this.LetterSelected = false;
            // this.FlatStyle = FlatStyle.Standard;
            this.Text = string.Empty;
        }

        public void OnLetterSelected()
        {
           // this.FlatStyle = FlatStyle.Flat;
           // this.FlatAppearance.BorderColor = Color.LimeGreen;
           // this.FlatAppearance.BorderSize = 5;
            this.LetterSelected = true;
        }

        public void OnLetterDeselected()
        {
            //this.FlatStyle = FlatStyle.Standard;
            this.LetterSelected = false;
        }
    }
}
