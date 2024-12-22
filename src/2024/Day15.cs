namespace Klinkby.AoC2024;

public class Day15
{
    private readonly static string[] input = EmbeddedResource.day15_txt.ReadAllLines()
        .ToArray();

    private readonly static char[][] map = input
        .TakeWhile(s => s.Length > 0)
        .Select(static x => x.ToCharArray())
        .ToArray();

    private readonly static Size[] moves = input.SkipWhile(s => s.Length > 0).Skip(1)
        .SelectMany(static x => x.ToCharArray())
        .Select(static x => x switch
        {
            '^' => new Size(0, -1),
            '>' => new Size(1, 0),
            'v' => new Size(0, 1),
            '<' => new Size(-1, 0),
            _ => throw new ArgumentException(null, nameof(x))
        }).ToArray();

    [Test]
    public async Task Puzzle1()
    {
        Point pos = map.FindLocation('@');
        map[pos.Y][pos.X] = '.';
        foreach (Size move in moves)
        {
            Point next = pos + move;
            char nextChar = map[next.Y][next.X];
            pos = nextChar switch
            {
                '.' => next,
                'O' => PushBox(pos, move),
                _ => pos
            };
        }

        int sum = map.SelectMany((row, y) =>
                row.Select((ch, x) => (ch, new Point(x, y))))
            .Where(tile => tile.ch == 'O')
            .Sum(tile => tile.Item2.Y * 100 + tile.Item2.X);
        await Assert.That(sum).IsEqualTo(1475249);
    }

    private static Point PushBox(Point pos, Size move)
    {
        Point next = pos + move;
        char nextChar = map[next.Y][next.X];
        Point mover = next;
        while (nextChar != '#')
        {
            mover += move;
            nextChar = map[mover.Y][mover.X];
            if (nextChar != '.')
            {
                continue;
            }

            while (mover.X != next.X || mover.Y != next.Y)
            {
                map[mover.Y][mover.X] = 'O';
                mover -= move;
            }

            pos = next;
            map[pos.Y][pos.X] = '.';
            break;
        }

        return pos;
    }
}