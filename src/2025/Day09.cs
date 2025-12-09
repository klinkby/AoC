using System.Runtime.Intrinsics;

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
        Span<Vector64<int>> coordinates = stackalloc Vector64<int>[496];
        int count = 0;
        long max = 0;
        stream.Read('\n', coordinates, (text, c) =>
        {
            if (!text.TryParseVector(out c[count])) throw new InvalidOperationException("Bad input");
            
            var thisMax = Vector128.Create(max, 0);
            for (int i = count - 1; i >= 0; i--)
            {
                var wh = Vector64.Abs(c[i] - c[count]) + Vector64.Create(1);
                var area = Vector128.Create(wh[0], 0) * Vector128.Create(wh[1], 0);
                thisMax = Vector128.Max(area, thisMax);
            }

            max = thisMax[0];
            count++;
        });

        Assert.Equal(expected, max);
    }
}
