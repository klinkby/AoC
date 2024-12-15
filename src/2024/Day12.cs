namespace Klinkby.AoC2024;

public class Day12
{
    private readonly static char[][] map = EmbeddedResource.input_day12_txt
        .ReadAllLines()
            .Select(static x => x.ToCharArray())
            .ToArray();
    private readonly static Rectangle bounds = new(0, 0, map[0].Length, map.Length);
    private readonly static Size[] steps =
    [
        new(1, 0),
        new(0, 1),
        new(-1, 0),
        new(0, -1)
    ];

    [Test]
    public async Task Puzzle1()
    {
        List<(HashSet<Point> Area, HashSet<Rectangle> Boundary)> regions = GetRegions();
        long price = regions.Sum(static r => (long)r.Area.Count * r.Boundary.Count);
        await Assert.That(price).IsEqualTo(1465968);
    }
    
    private static List<(HashSet<Point> Area, HashSet<Rectangle> Boundary)> GetRegions()
    {
        List<(HashSet<Point> Area, HashSet<Rectangle> Boundary)> regions = [];
        for (int y = 0; y < bounds.Bottom; y++)
        {
            for (int x = 0; x < bounds.Right; x++)
            {
                Point pos = new(x, y);
                if (regions.Any(r => r.Area.Contains(pos)))
                {
                    continue;
                }

                var (area, boundary) = FloodFill(pos);
                regions.Add((area, boundary));
            }
        }

        return regions;
    }

    private static (HashSet<Point> Area, HashSet<Rectangle> Boundary) FloodFill(Point pos)
    {
        HashSet<Point> area = [pos];
        HashSet<Rectangle> boundary = [];
        Queue<Point> queue = new([pos]);
        char plant = map[pos.Y][pos.X];
        while (queue.TryDequeue(out Point p))
            foreach (Size s in steps)
            {
                Point ps = p + s;
                if (!bounds.Contains(ps) || map[ps.Y][ps.X] != plant)
                {
                    boundary.Add(new Rectangle(p, s));
                    continue;
                }

                if (area.Add(ps))
                {
                    queue.Enqueue(ps);
                }
            }

        return (area, boundary);
    }
}