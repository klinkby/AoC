using System.Runtime.InteropServices;

namespace Klinkby.AoC2025;

/// <summary>
///     Day 4: Printing Department
/// </summary>
public sealed class Day04
{
    private const int MaxAdjacentRolls = 3;
    private const char RollSymbol = '@';
    private const char MarkSymbol = '/';
    private const char EmptySymbol = '.';

    [Theory]
    [InlineData(1543)]
    public void Puzzle1(long expected)
    {
        using Stream stream = EmbeddedResource.day04_txt.GetStream();
        char[][] window = [[], [], []];
        // streaming aggregate 
        long sum = stream.ReadAggregate('\n', row =>
        {
            char[] rowClone = row.ToArray();
            ShiftDown(window, rowClone);
            return MarkAccessiblePaperRolls(window);
        });
        
        // read appended window row
        ShiftDown(window, []);
        sum += MarkAccessiblePaperRolls(window); 

        Assert.Equal(expected, sum);
    }

    [Theory]
    [InlineData(9038)]
    public void Puzzle2(long expected)
    {
        using Stream stream = EmbeddedResource.day04_txt.GetStream();
        List<char[]> buffer = new(150); // read all rows into buffer for manipulation
        stream.Read('\n', row => buffer.Add(row.ToArray()));
        buffer.Add([]); // for window row
        
        long sum = RemoveAccessiblePaperRolls( CollectionsMarshal.AsSpan(buffer));

        Assert.Equal(expected, sum);
    }

    private static long RemoveAccessiblePaperRolls(Span<char[]> rows)
    {
        long count = 0;
        
        // count + mark accessible rolls
        char[][] window = [[], [], []];
        foreach (char[] row in rows)
        {
            ShiftDown(window, row);
            count += MarkAccessiblePaperRolls(window);
        }
        
        // remove marked
        for (int y = rows.Length - 1; y >= 0; y--)
            rows[y].Replace(MarkSymbol, EmptySymbol);

        return count
               + (count != 0
                    ? RemoveAccessiblePaperRolls(rows) // recurse
                    : 0);
    }

    private static int MarkAccessiblePaperRolls(Span<char[]> buffer)
    {
        int count = 0;
        for (int x = buffer[1].Length - 1; x >= 0; x--)
        {
            // investigate buffer[1][x]  
            if (0 == HasRoll(buffer[1], x))
            {
                continue;
            }

            int adjacent
                = HasRoll(buffer[0], x - 1) + HasRoll(buffer[0], x) + HasRoll(buffer[0], x + 1)
                  + HasRoll(buffer[1], x - 1) + 0 + HasRoll(buffer[1], x + 1)
                  + HasRoll(buffer[2], x - 1) + HasRoll(buffer[2], x) + HasRoll(buffer[2], x + 1);
            bool accessible = adjacent <= MaxAdjacentRolls;
            if (!accessible)
            {
                continue;
            }

            buffer[1][x] = MarkSymbol;
            count++;
        }

        return count;
    }

    private static int HasRoll(char[] row, int x) =>
        x < 0 || x >= row.Length
            ? 0
            : row[x] == RollSymbol || row[x] == MarkSymbol
                ? 1
                : 0;

    private static void ShiftDown<T>(T[] buffer, T newRow)
    {
        for (int i = buffer.Length - 1; i > 0;)
            buffer[i] = buffer[--i];
        buffer[0] = newRow;
    }
}