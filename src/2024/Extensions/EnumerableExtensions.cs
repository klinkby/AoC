namespace Klinkby.AoC2024.Extensions;

internal static class EnumerableExtensions
{
    public static int[][] AsColumns(this IEnumerable<IReadOnlyList<int>> input, int columns)
    {
        return Enumerable
            .Range(0, columns)
            .Select(column => input.Select(x => x[column]).ToArray())
            .ToArray();
    }

    public static IEnumerable<List<int>> ParseInts(this IEnumerable<string> input, Regex separator)
    {
        return input.Parse(separator, text => int.Parse(text, CultureInfo.InvariantCulture));
    }

    public static IEnumerable<List<long>> ParseLongs(this IEnumerable<string> input, Regex separator)
    {
        return input.Parse(separator, text => long.Parse(text, CultureInfo.InvariantCulture));
    }

    private static IEnumerable<List<T>> Parse<T>(this IEnumerable<string> input, Regex separator, Func<ReadOnlySpan<char>, T> parse)
    {
        List<T> values = new(20);
        foreach (ReadOnlySpan<char> line in input)
        {
            if (!EnumerateSplits<T>(values, separator, line, parse))
            {
                continue;
            }

            yield return values;
            values.Clear();
        }
    }

    private static bool EnumerateSplits<T>(List<T> values, Regex separator, ReadOnlySpan<char> line, Func<ReadOnlySpan<char>, T> parse)
    { 
        foreach (Range match in separator.EnumerateSplits(line))
        {
            if (match.End.Value == 0)
            {
                return false;
            }

            values.Add(parse(line[match]));
        }

        return true;
    }
}