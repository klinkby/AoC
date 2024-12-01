using System.Globalization;
using System.Text.RegularExpressions;

namespace Klinkby.AoC2024.Extensions;

internal static class SpanExtensions
{
    public static int[][] ParseIntegerColumns(this ReadOnlySpan<string> input, int columnCount)
    {
        Regex regex = CreateIntegerColumnParser(columnCount);
        int rowCount = input.Length;
        int[][] integers = CreateJaggedArray(columnCount, rowCount);

        for (int i = 0; i < rowCount; i++)
        {
            Match match = regex.Match(input[i]);
            for (int column = 0; column < columnCount; column++)
            {
                integers[column][i] = int.Parse(match.Groups[column + 1].Value, CultureInfo.InvariantCulture);
            }
        }

        return integers;
    }

    private static int[][] CreateJaggedArray(int x, int y)
    {
        int[][] columns = new int[x][];
        for (int column = 0; column < x; column++)
        {
            columns[column] = new int[y];
        }

        return columns;
    }

    private static Regex CreateIntegerColumnParser(int columns)
    {
        string pattern = string.Join(@"\s+", Enumerable.Range(1, columns).Select(_ => @"(\d+)"));
        Regex regex = new(pattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
        return regex;
    }
}