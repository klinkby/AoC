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
        using Stream stream = EmbeddedResource.day02_txt.GetStream();
        long sum = stream.ReadAggregate(',', GetRepeatedValues);
        
        Assert.Equal(expected, sum);
    }

    private static long GetRepeatedValues(ReadOnlySpan<char> text)
    {
        long rangeSum = 0L;
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
                rangeSum += value;
            }
        }

        return rangeSum;
    }

    [Theory]
    [InlineData(31898925685)]
    public void Puzzle2(long expected)
    {
        using Stream stream = EmbeddedResource.day02_txt.GetStream();
        long sum = stream.ReadAggregate(',', GetMultiRepeatedValues);
        
        Assert.Equal(expected, sum);
    }

    private static long GetMultiRepeatedValues(ReadOnlySpan<char> text)
    {
        long rangeSum = 0L;
        (long from, long to) = ParseRange(text);
        Span<char> buffer = stackalloc char[20];
            
        for (long value = from; value <= to; value++)
        {
            value.TryFormat(buffer, out int length, provider: CultureInfo.InvariantCulture);
                
            for (int split = length >> 1; split >= 1; split--)
            {
                if (!IsAllMatches(buffer[.. length], split))
                {
                    continue;
                }

                rangeSum += value;
                break;
            }
        }

        return rangeSum;
    }

    private static bool IsAllMatches(ReadOnlySpan<char> buffer, int split)
    {
        if (buffer.Length % split != 0) return false;
        
        ReadOnlySpan<char> left = buffer[.. split];
        bool allMatches = true;
                    
        for (int index = split; index < buffer.Length; index += split)
        {
            ReadOnlySpan<char> other = buffer[index .. (index + split)];
            if (0 == left.CompareTo(other, StringComparison.Ordinal))
            {
                continue;
            }

            allMatches = false;
            break;
        }

        return allMatches;
    }

    private static (long From, long To) ParseRange(ReadOnlySpan<char> text)
    {
        int split = text.IndexOf('-');
        return (
            long.Parse(text[.. split], CultureInfo.InvariantCulture),
            long.Parse(text[(split + 1) ..], CultureInfo.InvariantCulture));
    }
}