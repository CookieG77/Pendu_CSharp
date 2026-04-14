using PenduSharp.Core.Parsing;

namespace PenduSharp.Core;

/**
 * Static class responsible for loading game assets such as ASCII images and word lists from files.
 *
 * Using a static class allows us to load the assets once and have them available globally throughout the application without needing to create an instance of the AssetLoader.
 * Nor load the assets manually in the Main method of the Pendu class, which keeps the Main method cleaner and more focused on initializing the game and menus.
 */
public static class AssetLoader
{
    public static readonly List<ASCIIImage> AsciiImages = FileParser.ParseMultiImageTextFile(Path.Combine(AppContext.BaseDirectory, "assets", "hangman.txt"), 8);
    public static readonly List<WordList> WordLists = FileParser.ParseMultiWordListFile(Path.Combine(AppContext.BaseDirectory, "assets", "wordLists"));
}