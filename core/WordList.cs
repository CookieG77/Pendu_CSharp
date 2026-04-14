using System.Text.Json;
using System.Text.Json.Serialization;

namespace PenduSharp.Core;

public class WordList(List<string> words, int index, string displayName)
{
    private static readonly Random WordListRandom = new();

    public string DisplayName { get; private set; } = displayName;
    public int Index { get; private set; } = index;
    public List<string> Words { get; private set; } = words;

    public string GetRandomWord()
    {
        return Words[WordListRandom.Next(Words.Count)];
    }
    
    /**
     * Creates a WordList from a JSON file. The JSON file should have the following format:
     * {
     *     "displayName": "My Word List",
     *     "words": [
     *          "word1",
     *          "word2",
     *          "word3"
     *     ]
     * }
     */
    public static WordList FromFile(string path)
    {
        var json = File.ReadAllText(path);
        
        var wordListData = JsonSerializer.Deserialize<WordListData>(json);
        
        if (wordListData == null)
            throw new FileNotFoundException("Failed to deserialize the word list data : ", path);
        
        if (string.IsNullOrWhiteSpace(wordListData.DisplayName))
            throw new Exception($"The word list file at {path} is missing a display name.");
        
        if (wordListData.Words == null || wordListData.Words.Count == 0)
            throw new Exception($"The word list file at {path} does not contain any words.");
        
        return new WordList(
            wordListData.Words,
            wordListData.Index,
            wordListData.DisplayName
            );
    }

    private class WordListData
    {
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = "";
        
        [JsonPropertyName("index")]
        public int Index { get; set; } = 9999; // If index is not provided, set it to a high value to ensure it appears at the end of the list
        
        [JsonPropertyName("words")]
        public List<string> Words { get; set; } = [];
    }
}