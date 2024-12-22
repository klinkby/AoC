namespace Klinkby.AoC2024;

public class Day10
{
    private static readonly char[][] tiles = EmbeddedResource.day10_txt
        .ReadAllLines()
        .Select(static x => x.ToCharArray())
        .ToArray();
    private static readonly Rectangle bounds = new(0, 0, tiles[0].Length, tiles.Length);
    private static readonly Size[] steps =
    [
        new(1, 0),
        new(0, 1),
        new(-1, 0),
        new(0, -1)
    ];

    [Test]
    public async Task Puzzle1()
    {
        IEnumerable<Point> trialheads =
            from y in Enumerable.Range(0, tiles.Length)
            from x in Enumerable.Range(0, tiles[0].Length)
            where tiles[y][x] == '0'
            select new Point(x, y);
        int trials = trialheads
            .SelectMany(static pos => Score(pos).Distinct())
            .Count();
        await Assert.That(trials).IsEqualTo(510);
    }

    private static IEnumerable<Point> Score(Point pos)
    {
        char height = tiles[pos.Y][pos.X];
        if (height == '9')
        {
            return [pos];
        }

        height++;
        IEnumerable<Point> found = from s in steps
            let ps = pos + s
            where bounds.Contains(ps) && height == tiles[ps.Y][ps.X]
            from p in Score(ps)
            select p;
        return found;
    }
}