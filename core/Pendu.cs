namespace PenduSharp.Core;

public static class Pendu
{
    public static void Main()
    {
        Console.WriteLine("Hello World! AAAA");

        // var images = TxtImageParser.Parse(Path.Combine(AppContext.BaseDirectory, "assets", "hangman.txt"), 7);
        //
        // foreach (var image in images)
        // {
        //     Console.WriteLine("1111111111111");
        //     image.Print();
        // }
        
        var wordLists = FileParser.ParseMultiWordListFile(Path.Combine(AppContext.BaseDirectory, "assets", "wordLists"));
        
        foreach (var wordList in wordLists)
            Console.WriteLine($" {wordList.Index}. {wordList.DisplayName} ({wordList.Words.Count} words)");
    }
}