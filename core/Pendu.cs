namespace PenduSharp.Core;

public class Pendu
{
    public static void Main()
    {
        Console.WriteLine("Hello World! AAAA");

        var images = TxtImageParser.Parse(Path.Combine(AppContext.BaseDirectory, "assets", "hangman.txt"), 7);

        foreach (var image in images)
        {
            Console.WriteLine("1111111111111");
            image.Print();
        }
    }
}