// Read the input text file, parse the instructions and directions.
// Get a list of the positions that ends with an A.
// Iterate through the instructions, look up next direction for positions
// until all positions ends with a Z end, while counting the steps for each path.
// Calculate the least common multiple of the steps to find out when
// they all end on Z.

using System.Collections.Immutable;
using MathNet.Numerics;

var regInstr = new Regex(@"^(?'key'\w{3})\W+(?'L'\w{3})\W+(?'R'\w{3})",
    RegexOptions.Compiled | RegexOptions.ExplicitCapture);

using var reader = new StreamReader(@"../input.txt");
var instructions = reader.ReadLine()!.ToCharArray();
var directionMap = (
    from line in reader.ReadToEnd().Split('\n')
    let match = regInstr.Match(line)
    where match.Success
    select (
        key: match.Groups["key"].Value,
        L: match.Groups["L"].Value,
        R: match.Groups["R"].Value
    )
).ToImmutableSortedDictionary(x => x.key, x => (x.L, x.R));

var lengths = new List<long>();
foreach (var current in directionMap.Keys.Where(x => 'A' == x[^1]))
{
    var count = 0;
    var next = current;
    foreach (var ch in Loop(instructions))
    {
        count++;
        var direction = directionMap[next];
        next = 'L' == ch
            ? direction.L
            : direction.R;
        var atEnd = 'Z' == next[^1];
        if (atEnd)
        {
            lengths.Add(count);
            break;
        }
    }
}

var lcm = Euclid.LeastCommonMultiple(lengths);
Debug.Assert(9177460370549 == lcm);
Console.WriteLine(lcm);

static IEnumerable<T> Loop<T>(IReadOnlyCollection<T> collection)
{
    while (true)
        foreach (var item in collection)
            yield return item;
}