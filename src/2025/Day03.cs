namespace Klinkby.AoC2025;

/// <summary>
///     Day 3: Lobby
/// </summary>
public sealed class Day03
{
    [Theory]
    [InlineData(17435)]
    public void Puzzle1(long expected)
    {
        const int Batteries = 2;
        using Stream stream = EmbeddedResource.day03_txt.GetStream();
        long sum = stream.ReadAggregate('\n', bank =>
        {
            Span<char> max = stackalloc char[Batteries];
            MaximizeJoltage(ref max, ref bank);
            return long.Parse(max, CultureInfo.InvariantCulture);
        });

        Assert.Equal(expected, sum);
    }

    [Theory]
    [InlineData(172886048065379)]
    public void Puzzle2(long expected)
    {
        const int Batteries = 12;
        using Stream stream = EmbeddedResource.day03_txt.GetStream();
        long sum = stream.ReadAggregate('\n', bank =>
        {
            Span<char> max = stackalloc char[Batteries];
            MaximizeJoltage(ref max, ref bank);
            return long.Parse(max, CultureInfo.InvariantCulture);
        });

        Assert.Equal(expected, sum);
    }

    private static void MaximizeJoltage(ref readonly Span<char> max, ref readonly ReadOnlySpan<char> bank)
    {
        ReadOnlySpan<char> search = bank[.. ^(max.Length - 1)];
        int index = -1;
        char digit;
        // look for the largest digit in remaining bank that can still hold required number of digits  
        for (digit = '9'; digit >= '0' && (index = search.IndexOf(digit)) < 0; digit--)
            ;
        // found it!
        max[0] = digit;
        if (max.Length == 1)
        {
            return;
        }

        Span<char> remainingMax = max[1..]; // remaining buffer to fill
        ReadOnlySpan<char> remainingBank = bank[(index + 1) ..]; // remaning bank to search
        MaximizeJoltage(ref remainingMax, ref remainingBank);
    }
}