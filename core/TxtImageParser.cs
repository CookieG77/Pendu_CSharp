namespace PenduSharp.Core;

public class TxtImageParser
{
    /**
     * Parses a text file containing multiple mult-line ASCII art images and returns a list of ASCII images
     */
    public static List<ASCIIImage> Parse(string filePath, int imageLineCount)
    {
        var images = new List<ASCIIImage>();
        var lines = File.ReadAllLines(filePath).ToList();
        
        for (var i = 0; i < lines.Count; i += imageLineCount)
        {
            var imageLines = lines.Skip(i).Take(imageLineCount).ToList();
            images.Add(new ASCIIImage(imageLines));
        }
        
        return images;
    }
}