// Read the input text file, and parse the numbers on each line.
// Build a stack of the differences between the numbers until zeros.
// Then pop list while extrapolating the differences to the start.
// Sum the extrapolated values.

var regex = new Regex(@"(\-?\d+)",
    RegexOptions.Compiled | RegexOptions.ExplicitCapture);

var points =
    from line in File.ReadAllLines(@"../input.txt")
    select regex.Matches(line).Select(m => int.Parse(m.Value)).ToArray();

var sum = points.Select(Extrapolate).Sum();
Debug.Assert(1053 == sum);
Console.WriteLine(sum);

int Extrapolate(int[] diff)
{
    var queue = new Stack<int[]>();
    do
    {
        queue.Push(diff);
        diff = new int[diff.Length - 1];
    } while (!FillDiffs(queue.Peek(), diff));

    var delta = 0;
    while (queue.TryPop(out var current))
    {
        delta = current[0] -= delta; // <-- only change from puzzle 1
    }
    return delta;
}

bool FillDiffs(ReadOnlySpan<int> source, Span<int> target)
{
    bool allZero = true;
    for (var i = 1; i < source.Length; i++)
        allZero &= 0 == (target[i - 1] = source[i] - source[i - 1]);
    return allZero;
}