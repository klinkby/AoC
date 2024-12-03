using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Klinkby.AoC2024.Extensions;

internal static partial class EmbeddedResourceExtensions
{
    public static string ReadToEnd(this EmbeddedResource embeddedResource)
    {
        using Stream stream = embeddedResource.GetStream();
        using StreamReader sr = new(stream, Encoding.UTF8, leaveOpen: true);
        return sr.ReadToEnd();
    }
    
    public static IReadOnlyList<IReadOnlyList<int>> ReadAllIntegers(this EmbeddedResource embeddedResource)
    {
        List<IReadOnlyList<int>> rows = new(1000);
        rows.AddRange(ReadLines(embeddedResource).Select(ParseCells));

        return rows;
    }

    private static IEnumerable<string> ReadLines(this EmbeddedResource embeddedResource)
    {
        using Stream stream = embeddedResource.GetStream();
        using StreamReader sr = new(stream, Encoding.UTF8, leaveOpen: true);
        while (!sr.EndOfStream)
        {
            string? str = sr.ReadLine();
            if (str is null)
            {
                continue;
            }

            yield return str;
        }
    }

    private static List<int> ParseCells(string line)
    {
        List<int> cells = new(10);
        foreach (Range range in SpaceSplitter().EnumerateSplits(line))
        {
            int integer = int.Parse(line[range], CultureInfo.CurrentCulture);
            cells.Add(integer);
        }

        return cells;
    }
    
    [GeneratedRegex(@"\s+", RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex SpaceSplitter();
}