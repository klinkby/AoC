// Read the input text file, parse into concatenatecd time and distance.
// Solve the hyperbola equation for tHold, quantize and subtract
// to find the margin to win the race.

using Klinkby.Toolkitt;

var regex = new Regex(@"(\d+)", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
(
    from line in File.ReadAllLines(@"../input.txt")
    select regex
        .Matches(line)
        .SelectMany(m => m.Groups[0].Captures.Select(x => long.Parse(x.Value)))
        .Aggregate((a, b) => long.Parse($"{a}{b}"))
).TryToValueTuple(out (long time, long distance) data);

var x = Math.Sqrt(Math.Pow(data.time, 2) - 4 * data.distance);
var tHold = (
    (int)Math.Ceiling((data.time - x) / 2),
    (int)Math.Floor((data.time + x) / 2)
);
var margin = tHold.Item2 - tHold.Item1 + 1;

Debug.Assert(21039729 == margin);
Console.WriteLine(margin);