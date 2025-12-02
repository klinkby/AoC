namespace Klinkby.AoC2025;

/// <summary>
///     Day 2: The Gift Shop
/// </summary>
public sealed class Day02
{
    [Theory]
    [InlineData(30608905813)]
    public void Puzzle1(long expected)
    {
        long sum = 0;
        
        using Stream stream = EmbeddedResource.day02_txt.GetStream();
        stream.Read(',', text =>
        {
            (long from, long to) = ParseRange(text);
            Span<char> buffer = stackalloc char[20];
            for (long value = from; value <= to; value++)
            {
                value.TryFormat(buffer, out int length, provider: CultureInfo.InvariantCulture);

                int split = length >> 1;
                ReadOnlySpan<char> left = buffer[..split];
                ReadOnlySpan<char> right = buffer[split..length];
                if (0 == left.CompareTo(right, StringComparison.Ordinal))
                {
                    sum += value;
                }
            }
        });
        
        Assert.Equal(expected, sum);
    }

    [Theory]
    [InlineData(31898925685)]
    public void Puzzle2(long expected)
    {
        long sum = 0;
        
        using Stream stream = EmbeddedResource.day02_txt.GetStream();
        stream.Read(',', text =>
        {
            (long from, long to) = ParseRange(text);
            Span<char> buffer = stackalloc char[20];
            for (long value = from; value <= to; value++)
            {
                value.TryFormat(buffer, out int length, provider: CultureInfo.InvariantCulture);
                
                for (int split = length >> 1; split >= 1; split--)
                {
                    if (length % split != 0)
                    {
                        continue;
                    }

                    ReadOnlySpan<char> left = buffer[.. split];
                    bool allMatches = true;
                    
                    for (int index = split; index < length; index += split)
                    {
                        ReadOnlySpan<char> other = buffer[index .. (index + split)];
                        if (0 == left.CompareTo(other, StringComparison.Ordinal))
                        {
                            continue;
                        }

                        allMatches = false;
                        break;
                    }

                    if (!allMatches)
                    {
                        continue;
                    }

                    sum += value;
                    break;
                }
            }
        });
        
        Assert.Equal(expected, sum);
    }

    private static (long From, long To) ParseRange(ReadOnlySpan<char> text)
    {
        int split = text.IndexOf('-');
        return (
            long.Parse(text[.. split], CultureInfo.InvariantCulture),
            long.Parse(text[(split + 1) ..], CultureInfo.InvariantCulture));
    }
}