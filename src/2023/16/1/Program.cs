// Read the input text file and parse matrix contents.
// Induce a beam from the top left corner, and move it, energize its path until it exits the matrix.
// Push to a stack of beams when a splitter is encountered (but not if already followed).
// Keep popping until the stack is empty.
// Summarize the number of energized cells.

var input = File.ReadAllLines(@"../input.txt");
var size = new Size(input[0].Length, input.Length);
var bounds = new Rectangle(Point.Empty, size);
var model = new Contents[size.Height, size.Width];
for (var y = 0; y < size.Height; y++)
for (var x = 0; x < size.Width; x++)
    model[y, x] = input[y][x] switch
    {
        '|' => Contents.Splitter | Contents.Vertical,
        '-' => Contents.Splitter | Contents.Horizontal,
        '\\' => Contents.Mirror | Contents.Downward,
        '/' => Contents.Mirror | Contents.Upward,
        _ => 0
    };

var beams = new BeamManager();
beams.Push((new Point(-1, 0), new Size(1, 0)));
while (beams.TryPop(out var beam))
while (bounds.Contains(beam.position += beam.direction))
{
    var contents = model[beam.position.Y, beam.position.X] |= Contents.Energized;
    if ((contents & Contents.Mirror) != 0)
    {
        beam.direction = (contents & (Contents.Upward | Contents.Downward)) switch
        {
            Contents.Upward => new Size(-beam.direction.Height, -beam.direction.Width),
            Contents.Downward => new Size(beam.direction.Height, beam.direction.Width),
            _ => throw new InvalidOperationException()
        };
    }
    else if ((contents & Contents.Splitter) != 0 &&
             (((contents & Contents.Horizontal) != 0 && 0 != beam.direction.Height)
              || ((contents & Contents.Vertical) != 0 && 0 != beam.direction.Width)))
    {
        beams.Push((beam.position, new Size(beam.direction.Height, beam.direction.Width)));
        beams.Push((beam.position, new Size(-beam.direction.Height, -beam.direction.Width)));
        break;
    }
}

var sum = (
    from y in Enumerable.Range(0, size.Height)
    from x in Enumerable.Range(0, size.Width)
    where (model[y, x] & Contents.Energized) != 0
    select 1
).Sum();

Debug.Assert(7477 == sum);
Console.WriteLine(sum);

internal class BeamManager
{
    private readonly HashSet<(Point position, Size direction)> _done = [];
    private readonly Stack<(Point position, Size direction)> _todo = new();

    public void Push((Point position, Size direction) beam)
    {
        if (!_done.Add(beam)) return; // been there done that
        _todo.Push(beam);
    }

    public bool TryPop(out (Point position, Size direction) beam)
    {
        return _todo.TryPop(out beam);
    }
}

[Flags]
internal enum Contents : byte
{
    Mirror = 1,
    Splitter = 2,
    Upward = 4,
    Downward = 8,
    Horizontal = 16,
    Vertical = 32,
    Energized = 64
}