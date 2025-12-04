namespace Klinkby.AoC2025;

/// <summary>
///     Day 4: Printing Department
/// </summary>
public sealed class Day04
{
    private const int MaxAdjacentRolls = 3; 
    
    [Theory]
    [InlineData(1543)]
    public void Puzzle1(long expected)
    {
        using Stream stream = EmbeddedResource.day04_txt.GetStream();
        char[][] buffer = [[], [], []];
        long sum = stream.ReadAggregate('\n', row =>
        {
            Shift(buffer, row);
            return CountAccessiblePaperRolls(buffer);
        });
        for (int i = buffer.Length - 1; i > 0; i--)
        {
            Shift(buffer, []);
            sum += CountAccessiblePaperRolls(buffer);
        }

        Assert.Equal(expected, sum);
    }

    private static long CountAccessiblePaperRolls(char[][] buffer)
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
                  + HasRoll(buffer[1], x - 1) + 0                     + HasRoll(buffer[1], x + 1)
                  + HasRoll(buffer[2], x - 1) + HasRoll(buffer[2], x) + HasRoll(buffer[2], x + 1);
            
            count += adjacent <= MaxAdjacentRolls ? 1 : 0;
        }

        return count;
    }

    private static int HasRoll(char[] row, int x) =>
        x < 0 || x >= row.Length 
            ? 0 
            : row[x] == '@' 
                ? 1 
                : 0;

    private static void Shift(char[][] buffer, ReadOnlySpan<char> newRow)
    {
        for (int i = 2; i > 0;)
            buffer[i] = buffer[--i];
        buffer[0] = newRow.ToArray(); // heap clone
    }
}