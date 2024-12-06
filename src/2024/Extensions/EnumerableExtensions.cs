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
        List<int> values = new(20);
        foreach (ReadOnlySpan<char> line in input)
        {
            if (!EnumerateSplits(values, separator, line))
            {
                continue;
            }

            yield return values;
            values.Clear();
        }
    }

    private static bool EnumerateSplits(List<int> values, Regex separator, ReadOnlySpan<char> line)
    {
        foreach (Range match in separator.EnumerateSplits(line))
        {
            if (match.End.Value == 0)
            {
                return false;
            }

            values.Add(int.Parse(line[match], CultureInfo.InvariantCulture));
        }

        return true;
    }
}