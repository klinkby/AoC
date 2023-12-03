// Read the input text file,
// Parse the lines into tokens (gear symbols and numbers),
// exactly like in 03/1/Program.cs
// but this time find the gears that intersects with exactly two numbers.
// Multiply the two numbers and sum them up.

const int expectedRatios = 2;
var regex = new Regex(@"(?'gear'\*)|(?'number'\d+)",
    RegexOptions.Compiled | RegexOptions.ExplicitCapture);
var lines = File.ReadAllLines(@"../input.txt");

var tokens = (
    from y in Enumerable.Range(0, lines.Length)
    let line = lines[y]
    from match in regex.Matches(line)
    select new Token(match, y, match.Groups["number"].Success)
).ToArray();

var allNumbers = tokens.Where(t => t.IsNumber).ToArray();

var parts =
    from gear in tokens.Where(t => !t.IsNumber)
    let ratios = (
        from n in allNumbers
        where n.Bounds.IntersectsWith(gear.Bounds)
        select n.Value
    ).ToArray()
    let isGear = expectedRatios == ratios.Count()
    select isGear ? ratios[0] * ratios[1] : 0;

var sum = parts.Sum();
Debug.Assert(86879020 == sum);
Console.WriteLine(sum);