namespace Klinkby.AoC2025;

/// <summary>
///     Day 12: Christmas Tree Farm
/// </summary>
public sealed partial class Day12
{
    [Theory]
    [InlineData(440)]
    public void Puzzle1(long expected)
    {
        using var stream = EmbeddedResource.day12_txt.GetStream();
        Span<int> shapeBuffer = stackalloc int[6];
        var shapeSizeCount = 0;
        var shapeArea = 0;
        var sum = stream.ReadAggregate('\n', shapeBuffer, (text, shapeSizes) =>
        {
            (int W, int H, int ShapesArea) region = ParseRegion(text, shapeSizes);
            if (region.ShapesArea == 0)
            {
                // then it must be a shape
                var rowArea = text.Count('#');
                if (rowArea != 0)
                {
                    shapeArea += rowArea;
                }
                else if (shapeArea != 0)
                {
                    shapeSizes[shapeSizeCount++] = shapeArea;
                    shapeArea = 0;
                }

                return 0;
            }

            return region.W * region.H >= region.ShapesArea ? 1 : 0;
        });

        Assert.Equal(expected, sum);
        return;

        ValueTuple<int, int, int> ParseRegion(ReadOnlySpan<char> text, Span<int> shapes)
        {
            var m = RegionSplit().EnumerateSplits(text);
            if (!m.MoveNext()) return default; // not a region

            var range = m.Current;
            _ = text[.. range.End].TryParseInt(out var w);
            m.MoveNext();
            range = m.Current;
            _ = text[range.Start .. range.End].TryParseInt(out var h);
            if (h == 0) return default; // not a region

            m.MoveNext();
            range = m.Current;

            var dText = text[range.Start .. range.End];
            var area = 0;
            var shapeIndex = 0;
            foreach (var n in dText.Split(' '))
            {
                if (!dText[n.Start .. n.End].TryParseInt(out var count)) continue;
                area += shapes[shapeIndex++] * count; // add #-count to the shape area
            }

            return new ValueTuple<int, int, int>(w, h, area);
        }
    }

    [GeneratedRegex(@"[x|:]", RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex RegionSplit();
}