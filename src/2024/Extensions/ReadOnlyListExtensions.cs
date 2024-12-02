namespace Klinkby.AoC2024.Extensions;

internal static class ReadOnlyListExtensions
{
    public static int[][] AsColumns(this IReadOnlyList<IReadOnlyList<int>> input, int columns) =>
        Enumerable
            .Range(0, columns)
            .Select(column => input.Select(x => x[column]).ToArray())
            .ToArray();
}