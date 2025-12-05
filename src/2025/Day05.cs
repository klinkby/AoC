namespace Klinkby.AoC2025;

/// <summary>
///     Day 5: Cafeteria
/// </summary>
public sealed class Day05
{
    private delegate bool Strategy(ReadOnlySpan<char> text, List<LongRange> ranges, out Strategy next);

    [Theory]
    [InlineData(690)]
    public void Puzzle1(long expected)
    {
        using Stream stream = EmbeddedResource.day05_txt.GetStream();
        List<LongRange> ranges = new(200);
        Strategy strategy = AddRange; // switch to Search() when value-part is detected
        long sum = stream.ReadAggregate('\n', text => 
            strategy(text, ranges, out strategy) ? 1 : 0);

        Assert.Equal(expected, sum);
    }

    private static bool AddRange(ReadOnlySpan<char> text, List<LongRange> ranges, out Strategy next)
    {
        bool parsed = text.TryParseRange(out LongRange range);
        if (parsed) ranges.Add(range);
        next = parsed ? AddRange : Search;
        return false;
    }

    private static bool Search(ReadOnlySpan<char> text, List<LongRange> ranges, out Strategy next)
    {
        bool found = text.TryParseLong(out long value)
            && ranges.Exists(r => r.Contains(value));
        next = Search;
        return found;
    }
}