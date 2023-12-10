// Read the input text file to multidimensional char array.
// Find the start position, and keep moving, erasing our trail (so we don't go back)
// until we can't move anymore.
// Half the move count to find the middle of the loop.

var data = File.ReadLines(@"../input.txt")
    .Select(x => x.ToCharArray())
    .ToArray();
var size = new Size(data[0].Length, data.Length);
var startIndex = Array.IndexOf(data.SelectMany(x => x).ToArray(), 'S');
var start = new Point(startIndex % size.Width, startIndex / size.Width);

var moves = new[]
{
    (direction: new Size(-1, 0), current: "-J7S", next: "-LF"), // left
    (direction: new Size(1, 0), current: "-LFS", next: "-J7"), // right
    (direction: new Size(0, -1), current: "|LJS", next: "|7F"), // up
    (direction: new Size(0, 1), current: "|7FS", next: "|LJ") // down
};

Point? pos = start;
var count = 0;
var last = start;
do
{
    count++;
    pos = Move(pos.Value);
    if (!pos.HasValue)
        break;
    data[last.Y][last.X] = '#';
    last = pos.Value;
} while (true);

count /= 2;
Debug.Assert(7173 == count);
Console.WriteLine(count);

char? GetSymbol(Point loc)
{
    return loc.X < 0 || loc.X >= size.Width || loc.Y < 0 || loc.Y >= size.Height
        ? null
        : data![loc.Y][loc.X];
}

Point? Move(Point current)
{
    var here = GetSymbol(current)!.Value;
    foreach (var m in moves)
    {
        var symbol = GetSymbol(current + m.direction);
        var canMove = symbol.HasValue &&
                      m.current.Contains(here) && m.next.Contains(symbol.Value);
        if (canMove) return current + m.direction;
    }
    return default;
}