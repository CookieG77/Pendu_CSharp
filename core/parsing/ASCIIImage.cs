using System;
using System.Collections.Generic;
using System.Linq;

namespace PenduSharp.Core.Parsing;

/**
 * Represents an ASCII image, which is a multi-line string that can be used to display a visual representation of something in the console.
 */
public class ASCIIImage
{
    public List<string> Lines { get; private set; }
    
    public ASCIIImage(List<string> lines)
    {
        Lines = lines;
    }

    public void Print()
    {
        foreach (var line in Lines)
            Console.WriteLine(line);
    }
    
    public static ASCIIImage FromString(string asciiArt)
    {
        var lines = asciiArt.Split([Environment.NewLine], StringSplitOptions.None).ToList();
        return new ASCIIImage(lines);
    }
}