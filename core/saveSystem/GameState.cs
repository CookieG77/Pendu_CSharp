namespace PenduSharp.Core;

public class GameState
{
    public string Word { get; set; } = "";
    public List<char> GuessedLetters { get; set; } = new();
    public int AttemptsLeft { get; set; }
}