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
        long sum = stream.ReadAggregate('\n', stackalloc bool[150], (text, beams) =>
        {
            var index = text.IndexOf(StartSymbol);
            if (index != -1) beams[index] = true;

            var count = 0;
            foreach (var match in Splitters().EnumerateMatches(text))
            {
                if (!beams[match.Index]) continue;
                beams[match.Index] = false;
                beams[match.Index - 1] = beams[match.Index + 1] = true;
                count++;
            }

            return count;
        });
        
        Assert.Equal(expected, sum);
    }

    [GeneratedRegex(@"\^", RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex Splitters();
}