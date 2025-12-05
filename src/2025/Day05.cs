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
    
    [Theory]
    [InlineData(344323629240733)]
    public void Puzzle2(long expected)
    {
        using Stream stream = EmbeddedResource.day05_txt.GetStream();
        List<LongRange> ranges = new(200);
        stream.Read('\n', text => 
            MergeRange(text, ranges));
        
        long sum = ranges.Aggregate(0L, (agg, range) => 
            agg + range.To - range.From + 1);
        
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
    
    private static void MergeRange(ReadOnlySpan<char> text, List<LongRange> ranges)
    {
        if (!text.TryParseRange(out LongRange range)) return;

        for (int i = ranges.Count - 1; i >= 0; i--)
        {
            if (!ranges[i].Overlaps(range)) continue;

            range = new(
                Math.Min(range.From, ranges[i].From), 
                Math.Max(range.To, ranges[i].To));
            ranges.RemoveAt(i);
        }

        ranges.Add(range);
    }
}