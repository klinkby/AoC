namespace Klinkby.AoC2025;

/// <summary>
///     Day 8: Playground
/// </summary>
public sealed class Day08
{
    private static readonly Comparer<long> Comparer = Comparer<long>.Create((x, y) => x.CompareTo(y));
    
    [Theory]
    [InlineData(50568, 1000, 3)]
    public void Puzzle1(int expected, int limitDistances, int limitCircuits)
    {
        Span<(int X, int Y, int Z)> coords = stackalloc (int X, int Y, int Z)[1000];
        var positionCount = 0;
        using var stream = EmbeddedResource.day08_txt.GetStream();
        stream.Read('\n', coords, (text, c) =>
        {
            ParseCoords(text, c, positionCount);
            positionCount++;
        });
        coords = coords[..positionCount];
        
        var distances = CalculateDistances(coords, limitDistances);
        var sizes = MeasureSizes(distances, limitDistances);
        var mul = sizes
            .AsValueEnumerable()
            .OrderDescending()
            .Take(limitCircuits)
            .Aggregate(1L, (agg, count) => agg * count);

        Assert.Equal(expected, mul);
    }

    private static SortedList<long, (int A, int B)> CalculateDistances(Span<(int X, int Y, int Z)> coords, int limitDistances)
    {
        var coordsLength = coords.Length;
        SortedList<long, (int A, int B)> distances = new(limitDistances + 1, Comparer);
        for (var i = 0; i < coordsLength; i++)
        for (var j = i + 1; j < coordsLength; j++)
        {
            distances.Add(SquareDistance(ref coords[i], ref coords[j]), (i, j));
            if (distances.Count > limitDistances) distances.RemoveAt(limitDistances);
        }
        return distances;
    }

    private static List<int> MeasureSizes(SortedList<long, (int A, int B)> distances, int limitDistances)
    {
        var dsu = new DisjointSetUnion(stackalloc int[limitDistances], stackalloc int[limitDistances]);
        foreach (var distance in distances)
            dsu.Union(distance.Value.A, distance.Value.B);

        List<int> sizes = new(limitDistances / 3);
        HashSet<int> seen = new(sizes.Capacity);
        for (var i = 0; i < limitDistances; i++)
        {
            var root = dsu.Find(i);
            if (seen.Add(root)) sizes.Add(dsu.Count(root));
        }

        return sizes;
    }

    private static void ParseCoords(ReadOnlySpan<char> text, Span<(int X, int Y, int Z)> pos, int positionCount)
    {
        Span<int> xyz = stackalloc int[3];
        var i = 0;
        foreach (var range in text.Split(',')) 
            _ = text[range.Start .. range.End].TryParseInt(out xyz[i++]);
        pos[positionCount] = (xyz[0], xyz[1], xyz[2]);
    }

    private static long SquareDistance(ref readonly (int X, int Y, int Z) p1, ref readonly (int X, int Y, int Z) p2)
    {
        int dx = p1.X - p2.X, dy = p1.Y - p2.Y, dz = p1.Z - p2.Z;
        var d = (long)dx * dx + (long)dy * dy + (long)dz * dz;
        return d;
    }
}