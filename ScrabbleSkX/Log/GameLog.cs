namespace ScrabbleSkX.Log
{
    // Simple game log to store messages
    public class GameLog
    {
        public Stack<string> messages { get; set; } = new Stack<string>();

        public void Log(string msg)
        {
            messages.Push(msg);
        }
    }
}