namespace Klinkby.AoC2024;

public class Day14
{
    private readonly static Rectangle bounds = new(0, 0, 101, 103);
    private readonly static Size quadrantSize = bounds.Size / 2;

    [Test]
    public async Task Puzzle1()
    {
        Rectangle[] vectors = GetRobotVectors();
        AnimateRobots(vectors, 100);
        Rectangle[] quadrants =
        [
            new(Point.Empty, quadrantSize),
            new(new Point(bounds.Width - quadrantSize.Width, 0), quadrantSize),
            new(new Point(0, bounds.Height - quadrantSize.Height), quadrantSize),
            new(new Point(bounds.Width - quadrantSize.Width, bounds.Height - quadrantSize.Height), quadrantSize)
        ];
        int safetyFactor = quadrants
            .Select(q => vectors
                .Select(v => v.Location)
                .Count(q.Contains))
            .Aggregate(1, (i, agg) => i * agg);
        await Assert.That(safetyFactor).IsEqualTo(217132650);
    }

    [Test]
    public async Task Puzzle2()
    {
        Rectangle[] vectors = GetRobotVectors();
        Size slice = bounds.Size / 4;
        Rectangle middleSlice = new(
            new Point(quadrantSize.Width - slice.Width / 2,
                quadrantSize.Height - slice.Height / 2),
            slice);
        int trigger = vectors.Length / 5;
        int seconds = 0;
        bool isTrunkLike = false;
        while (!isTrunkLike)
        {
            AnimateRobots(vectors, 1);
            seconds++;
            isTrunkLike = vectors.Count(middleSlice.Contains) >= trigger;
        }

        DrawMap(vectors, middleSlice);
        
        await Assert.That(seconds).IsEqualTo(6516);
    }

    private static void DrawMap(Rectangle[] vectors, Rectangle slice)
    {
        char[][] map = Enumerable.Range(0, bounds.Height).Select(_ => new string('.', bounds.Width).ToCharArray())
            .ToArray();
        foreach (Rectangle v in vectors)
        {
            map[v.Y][v.X] = '#';
        }

        Array.ForEach<Point>(
        [
            new Point(slice.Top, slice.Left),
            new Point(slice.Top, slice.Right),
            new Point(slice.Bottom, slice.Left),
            new Point(slice.Bottom, slice.Right)
        ], p => map[p.Y][p.X] = '+');

        StringWriter logger = TestContext.Current!.OutputWriter;
        foreach (char[] line in map)
        {
            logger.WriteLine(new string(line));
        }
    }

    private static Rectangle[] GetRobotVectors()
    {
        return EmbeddedResource.input_day14_txt
            .ReadAllLines()
            .ParseInts()
            .Select(static v => new Rectangle(v[0], v[1], v[2], v[3]))
            .ToArray();
    }

    private static void AnimateRobots(Rectangle[] vectors, int seconds)
    {
        for (int i = 0; i < vectors.Length; i++)
        {
            Rectangle v = vectors[i];
            Point pos = v.Location + v.Size * seconds;
            vectors[i].Location = new Point(
                (pos.X % bounds.Width + bounds.Width) % bounds.Width,
                (pos.Y % bounds.Height + bounds.Height) % bounds.Height);
        }
    }
}