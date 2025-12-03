namespace Klinkby.AoC2025;

/// <summary>
///     Day 3: Lobby
/// </summary>
public sealed class Day03
{
    [Theory, InlineData(17435)]
    public void Puzzle1(long expected)
    {
        using Stream stream = EmbeddedResource.day03_txt.GetStream();
        
        long sum = stream.ReadAggregate('\n', text =>
        {
            Span<char> max = stackalloc char[2];
            Span<char> b = stackalloc char[2];
            
            for (int i = 0; i < text.Length; i++)
            {
                b[0] = text[i];
                for (int j = i + 1; j < text.Length; j++)
                {
                    b[1] = text[j];
                    if (b.CompareTo(max, StringComparison.Ordinal) > 0) b.CopyTo(max);
                }
            }
            
            return int.Parse(max, CultureInfo.InvariantCulture);
        });
        
        Assert.Equal(expected, sum);
    }
}