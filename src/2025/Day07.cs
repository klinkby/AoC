namespace Klinkby.AoC2025;

/// <summary>
///     Day 7: Laboratories
/// </summary>
public sealed partial class Day07
{
    private const char StartSymbol = 'S';
    
    [Theory]
    [InlineData(1600)]
    public void Puzzle1(long expected)
    {
        using Stream stream = EmbeddedResource.day07_txt.GetStream();
        long sum = stream.ReadAggregate('\n', stackalloc long[150], (text, beams) => 
            ProcessBeams(text, beams));
        
        Assert.Equal(expected, sum);
    }

    [Theory]
    [InlineData(8632253783011)]
    public void Puzzle2(long expected)
    {
        using Stream stream = EmbeddedResource.day07_txt.GetStream();
        Span<long> beams = stackalloc long[150];
        stream.Read('\n', beams, (text, b) => ProcessBeams(text, b));
        long sum = beams.AsValueEnumerable().Sum();
        
        Assert.Equal(expected, sum);
    }

    private static int ProcessBeams(ReadOnlySpan<char> text, Span<long> beams)
    {
        var index = text.IndexOf(StartSymbol);
        if (index != -1) beams[index] = 1;

        var count = 0;
        foreach (var match in Splitters().EnumerateMatches(text))
        {
            if (0 != beams[match.Index]) count++;
            
            beams[match.Index - 1] += beams[match.Index];
            beams[match.Index + 1] += beams[match.Index];
            beams[match.Index] = 0;
        }

        return count;
    }
    
    [GeneratedRegex(@"\^", RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex Splitters();
}