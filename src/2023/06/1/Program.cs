// Read the input text file, parse into time and distance.
// Solve the hyperbola equation for tHold, quantize and subtract
// to find the margin to win the race.
// Multiply the margins to find the power.
// tRace = duration - tHold = distance / tHold 

var regex = new Regex(@"(\d+)", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
using var reader = new StreamReader(@"../input.txt");
var (time, distance) = (ParseLine(reader), ParseLine(reader));

var margin = from i in Enumerable.Range(0, time.Count)
    let x = Math.Sqrt(Math.Pow(time[i], 2) - 4 * distance[i])
    let tHold = (
        (int)Math.Ceiling((time[i] - x) / 2),
        (int)Math.Floor((time[i] + x) / 2)
    )
    select tHold.Item2 - tHold.Item1 + 1;

var power = margin.Aggregate(1, (a, b) => a * b);
Debug.Assert(114400 == power);
Console.WriteLine(power);

IList<int> ParseLine(TextReader rd) => regex.Matches(rd.ReadLine()!).Select(m => int.Parse(m.Value)).ToArray();