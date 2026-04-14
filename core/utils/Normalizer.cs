using System.Globalization;
using System.Text;

namespace PenduSharp.Core.utils;

public class Normalizer
{
    public static string NormalizeStr(string input)
    {
        var normalized = input.Normalize(NormalizationForm.FormD);

        var sb = new StringBuilder();

        foreach (var character in normalized)
        {
            if (Char.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(character);
            }
        }
        
        return sb.ToString().ToLower();
    }
}