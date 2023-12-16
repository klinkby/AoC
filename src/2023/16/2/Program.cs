// Read the input text file and parse matrix contents.
// Like 16/1/Program.cs, but inducing beams from all around the matrix.
// See which one gives the highest number of energized cells.

var input = File.ReadAllLines(@"../input.txt");
var size = new Size(input[0].Length, input.Length);
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

int max = 0;
for (var y = 0; y < size.Height; y++)
{
    var beams = new BeamManager(model);
    max = Math.Max(max, beams.Energize((new Point(-1, y), new Size(1, 0))));
    beams = new BeamManager(model);
    max = Math.Max(max, beams.Energize((new Point(size.Width, y), new Size(-1, 0))));
}
for (var x = 0; x < size.Width; x++)
{
    var beams = new BeamManager(model);
    max = Math.Max(max, beams.Energize((new Point(x, -1), new Size(0, 1))));
    beams = new BeamManager(model);
    max = Math.Max(max, beams.Energize((new Point(x, size.Height), new Size(0, -1))));
}   

Debug.Assert(7853 == max);
Console.WriteLine(max);

internal class BeamManager
{
    private readonly HashSet<(Point position, Size direction)> _done = [];
    private readonly Stack<(Point position, Size direction)> _todo = new();
    private readonly Contents[,] _model;
    private readonly Size _size;
    private readonly Rectangle _bounds;
    
    public BeamManager(Contents[,] model)
    {
        _model = (Contents[,])model.Clone();
        _size = new Size(model.GetLength(1), model.GetLength(0));
        _bounds = new Rectangle(Point.Empty, _size);
    }

    public int Energize((Point position, Size direction) beam)
    {
        Push(beam);
        while (TryPop(out beam))
        while (_bounds.Contains(beam.position += beam.direction))
        {
            var contents = _model[beam.position.Y, beam.position.X] |= Contents.Energized;
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
                Push((beam.position, new Size(beam.direction.Height, beam.direction.Width)));
                Push((beam.position, new Size(-beam.direction.Height, -beam.direction.Width)));
                break;
            }
        }

        return (
            from y in Enumerable.Range(0, _size.Height)
            from x in Enumerable.Range(0, _size.Width)
            where (_model[y, x] & Contents.Energized) != 0
            select 1
        ).Sum();
    }

    private void Push((Point position, Size direction) beam)
    {
        if (!_done.Add(beam)) return; // been there done that
        _todo.Push(beam);
    }

    private bool TryPop(out (Point position, Size direction) beam)
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