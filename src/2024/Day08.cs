namespace Klinkby.AoC2024;

public class Day08
{
    private static readonly char[][] _map = EmbeddedResource.day08_txt.ReadAllLines()
        .Select(static x => x.ToCharArray())
        .ToArray();
    private static readonly Size _mapSize = new(_map[0].Length, _map.Length);

    [Test]
    public async Task Puzzle1()
    {
        IEnumerable<Point[]> antinodes =
            from char ch in GetAntennas()
            let antennas = GetAntennaPositions(ch).ToArray()
            from posIndex in antennas.Select(static (pos1, index) => (pos1, index))
            let pos1 = posIndex.pos1
            from pos2 in antennas.Skip(posIndex.index + 1)
            let delta = new Size(pos2.X - pos1.X, pos2.Y - pos1.Y)
            select new[] { pos1 - delta, pos2 + delta };
        int count = antinodes
            .SelectMany(static x => x)
            .Distinct()
            .Count(InBounds);

        await Assert.That(count).IsEqualTo(381);
    }

    private static IEnumerable<char> GetAntennas()
    {
        return _map.SelectMany(static x => x).Where(static ch => ch != '.').Distinct().Order();
    }

    private static IEnumerable<Point> GetAntennaPositions(char antenna)
    {
        return _map.SelectMany((row, y) =>
            row.Select((_, x) => new Point(x, y))
                .Where(p => _map[p.Y][p.X] == antenna));
    }

    private static bool InBounds(Point p)
    {
        return p is { X: >= 0, Y: >= 0 } && p.X < _mapSize.Width && p.Y < _mapSize.Height;
    }
}
