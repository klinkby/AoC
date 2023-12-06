// Read the input text file, parse into time and distance.
// Solve the hyperbola equation for tHold, quantize and subtract
// to find the margin to win the race.
// Multiply the margins to find the power.
// tRace = duration - tHold = distance / tHold 

using Klinkby.Toolkitt;

var regex = new Regex(@"(\d+)", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
(
    from line in File.ReadAllLines(@"../input.txt")
    select regex
        .Matches(line)
        .SelectMany(m => m.Groups[0].Captures.Select(x => int.Parse(x.Value)))
        .ToArray()
).TryToValueTuple(out (int[] time, int[] distance) data);

var margin = from i in Enumerable.Range(0, data.time.Length)
    let x = Math.Sqrt(Math.Pow(data.time[i], 2) - 4 * data.distance[i])
    let tHold = (
        (int)Math.Ceiling((data.time[i] - x) / 2),
        (int)Math.Floor((data.time[i] + x) / 2)
    )
    select tHold.Item2 - tHold.Item1 + 1;

var power = margin.Aggregate(1, (a, b) => a * b);
Debug.Assert(114400 == power);
Console.WriteLine(power);