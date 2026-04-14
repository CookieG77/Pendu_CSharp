using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PenduSharp.Core.Parsing;

public static class FileParser
{
    /**
     * Parses a text file containing multiple mult-line ASCII art images and returns a list of ASCII images
     */
    public static List<ASCIIImage> ParseMultiImageTextFile(string filePath, int imageLineCount)
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

    /**
     * Parse a folder containing multiple JSON files, each representing a WordList, and returns a list of WordLists
     */
    public static List<WordList> ParseMultiWordListFile(string folderPath)
    {
        var files = Directory.GetFiles(folderPath, "*.json");
        return files
            .Select(WordList.FromFile)
            .OrderBy(wordList => wordList.Index)
            .ThenBy(wordList => wordList.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}