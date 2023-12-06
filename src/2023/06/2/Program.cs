// Read the input text file, parse into concatenated time and distance.
// Solve the hyperbola equation for tHold, quantize and subtract
// to find the margin to win the race.

var regex = new Regex(@"(\d+)", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
using var reader = new StreamReader(@"../input.txt");
var (time, distance) = (ParseLine(reader), ParseLine(reader));

var x = Math.Sqrt(Math.Pow(time, 2) - 4 * distance);
var tHold1 = (int)Math.Ceiling((time - x) / 2);
var tHold2 = (int)Math.Floor((time + x) / 2);
var margin = tHold2 - tHold1 + 1;

Debug.Assert(21039729 == margin);
Console.WriteLine(margin);

long ParseLine(TextReader rd) => long.Parse(string.Join("", regex.Matches(rd.ReadLine()!).Select(m => m.Value)));