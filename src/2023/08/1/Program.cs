// Read the input text file, parse the instructions and directions.
// Iterate through the instructions, look up next direction
// until the end while counting the steps.

using System.Collections.Immutable;

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

var next = "AAA";
var count = 0;
foreach (var ch in RingIterator(instructions))
{
    var direction = directionMap[next];
    next = ch switch
    {
        'L' => direction.L,
        'R' => direction.R,
        _ => throw new Exception()
    };
    count++;
    if ("ZZZ" == next) break;
}

Debug.Assert(19783 == count);
Console.WriteLine(count);

static IEnumerable<T> RingIterator<T>(IReadOnlyCollection<T> collection)
{
    while (true)
        foreach (var item in collection)
            yield return item;
}