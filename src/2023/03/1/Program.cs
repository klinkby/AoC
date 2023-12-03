// Read the input text file,
// Parse the lines into tokens (symbols and numbers),
// filter the numbers that intersects symbols
// then summarize.

var regex = new Regex(@"(?'symbol'[^\d^\.])|(?'number'\d+)",
    RegexOptions.Compiled | RegexOptions.ExplicitCapture);
var lines = File.ReadAllLines(@"../input.txt");

var tokens = (
    from y in Enumerable.Range(0, lines.Length)
    let line = lines[y]
    from match in regex.Matches(line)
    select new Token(match, y, match.Groups["number"].Success)
).ToArray();

var allSymbols =
    tokens.Where(t => !t.IsNumber)
        .ToArray();

var parts =
    from number in tokens.Where(t => t.IsNumber)
    where allSymbols.Any(s => s.Bounds.IntersectsWith(number.Bounds))
    select number.Value;

var sum = parts.Sum();
Debug.Assert(540131 == sum);
Console.WriteLine(sum);