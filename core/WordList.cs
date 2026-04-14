namespace PenduSharp.Core;

public class WordList
{
    private static readonly Random WordListRandom = new();

    private List<string> Word;
    public string DisplayName { get; private set; }
    
    public WordList(List<string> word, string displayName)
    {
        Word = word;
        DisplayName = displayName;
    }
    
    public string GetRandomWord()
    {
        return Word[WordListRandom.Next(Word.Count)];
    }
    
    
}