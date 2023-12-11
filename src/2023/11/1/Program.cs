// Read the input text file into list of galaxy locations.
// Expand the universe by moving all galaxies after a spacer row or column.
// Find the route pairs and calculate the shortest path between them.
// Sum the lengths.

var lines = File.ReadLines(@"../input.txt");
var (galaxies, size) = ParseGalaxyList(lines);
ExpandUniverse(galaxies, size);
var paths =
    from pair in GetPairs(galaxies.Count)
    select GetShortestPath(galaxies[pair.Item1], galaxies[pair.Item2]);
var sum = paths.Sum();
Debug.Assert(10885634 == sum);
Console.WriteLine(sum);

int GetShortestPath(Point p1, Point p2) => 
    Math.Abs(p2.X - p1.X) + Math.Abs(p2.Y - p1.Y);

IEnumerable<(int, int)> GetPairs(int count) =>
    from i in Enumerable.Range(0, count)
    from j in Enumerable.Range(i + 1, count - i - 1)
    select (i, j);

(IList<Point> galaxies, Size size) ParseGalaxyList(IEnumerable<string> data)
{
    var regex = new Regex(@"([^\.])", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    var (x, y) = (0, 0);
    var positions =
        from row in data
        let colNo = x = (y == 0 ? x = row.Length : y)
        let rowNo = y++
        from match in regex.Matches(row)
        select new Point(match.Index, rowNo);
    return (positions.ToArray(), new Size(x, y));
}

void ExpandUniverse(IList<Point> list, Size extent)
{
    var hSpace = from i in Enumerable.Range(0, extent.Width).Reverse()
        where list.All(g => g.X != i)
        select i;
    foreach (var i in hSpace)
        for (var j = 0; j < list.Count; j++)
            if (list[j].X > i)
                list[j] += new Size(1, 0);

    var vSpace = from i in Enumerable.Range(0, extent.Height).Reverse()
        where list.All(g => g.Y != i)
        select i;
    foreach (var i in vSpace)
        for (var j = 0; j < list.Count; j++)
            if (list[j].Y > i)
                list[j] += new Size(0, 1);
}