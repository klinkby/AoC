namespace Klinkby.AoC2025;

/// <summary>
///     Day 9: Movie Theater
/// </summary>
public sealed class Day09
{
    [Theory]
    [InlineData(4781235324)]
    public void Puzzle1(long expected)
    {
        using Stream stream = EmbeddedResource.day09_txt.GetStream();
        Span<(int X, int Y)> coordinates = stackalloc ValueTuple<int, int>[496];
        int count = 0;
        long max = 0;

        stream.Read('\n', coordinates, (text, c) =>
        {
            if (!text.TryParseTuple(out c[count])) throw new InvalidOperationException("Bad input");
            for (int i = count - 1; i >= 0; i--)
            {
                long width = Math.Abs(c[i].X - c[count].X) + 1;
                int height = Math.Abs(c[i].Y - c[count].Y) + 1;
                max = Math.Max(max, width * height);
            }

            count++;
        });

        Assert.Equal(expected, max);
    }
}
