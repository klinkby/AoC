using System.Drawing;

namespace Klinkby.AoC2024;

public class Day06
{
    [Test]
    public async Task Puzzle1()
    {
        char[][] map = GetMap();
        AnimateGuard(FindStartPos(map), map, true);

        int sum = map.SelectMany(c => c).Count(c => c == 'X');
        await Assert.That(sum).IsEqualTo(5153);
    }

    [Test]
    public async Task Puzzle2()
    {
        char[][] map = GetMap();
        Point pos = FindStartPos(map);
        IEnumerable<Point> obstacles =
            from r in map.Select((row, y) => (row, y))
            from x in Enumerable.Range(0, r.row.Length)
            where r.row[x] == '.'
            select new Point(x, r.y);

        int sum = obstacles.Sum(o =>
        {
            map[o.Y][o.X] = '#';
            bool loop = AnimateGuard(pos, map);
            map[o.Y][o.X] = '.';
            return loop ? 1 : 0;
        });

        await Assert.That(sum).IsEqualTo(1711);
    }

    private static Point FindStartPos(char[][] map)
    {
        return (
            from r in map.Select((row, y) => (row, y))
            let x = Array.IndexOf(r.row, '^')
            where x >= 0
            select new Point(x, r.y)).Single();
    }

    private static bool AnimateGuard(Point pos, char[][] map, bool markTrial = false)
    {
        Size step = new(0, -1);
        HashSet<(Point, Size)> visited = [];
        try
        {
            while (visited.Add((pos, step)))
            {
                if (markTrial)
                {
                    map[pos.Y][pos.X] = 'X';
                }

                Point next = pos + step;
                if (map[next.Y][next.X] != '#')
                {
                    pos = next;
                    continue;
                }

                step = new Size(-step.Height, step.Width); // 90 degrees clockwise
            }

            return true; // loop detected
        }
        catch (IndexOutOfRangeException)
        {
            // guard left the map
        }

        return false;
    }

    private static char[][] GetMap()
    {
        return EmbeddedResource.input_06_txt
            .ReadAllLines()
            .Select(x => x.ToCharArray())
            .ToArray();
    }
}