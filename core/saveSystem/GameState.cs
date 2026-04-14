using System.Text.Json.Serialization;

namespace PenduSharp.Core.SaveSystem;

/**
 * Represents the state of the game at a given point in time, which can be saved to a file and loaded later to resume the game.
 */
public class GameState
{
    [JsonPropertyName("word")]
    public string Word { get; set; } = "";
    
    [JsonPropertyName("guessedLetters")]
    public List<char> GuessedLetters { get; set; } = [];
    
    [JsonPropertyName("attemptsLeft")]
    public int AttemptsLeft { get; set; }
}