// Read the input text file,
// Parse the lines into winning numbers and your numbers,
// Intersect the two sets of numbers,
// calculate points as 2^count,
// then summarize.

var regex = new Regex(@"(?::)(?:\s+(?'win'\d+))+(?:\s\|)(?:\s+(?'you'\d+))+$",
    RegexOptions.Compiled | RegexOptions.ExplicitCapture);

var points =
    from line in File.ReadAllLines(@"..\..\..\..\input.txt")
    from m in regex.Matches(line)
    let win = m.Groups["win"].Captures.Select(c => int.Parse(c.Value))
    let you = m.Groups["you"].Captures.Select(c => int.Parse(c.Value))
    let count = win.Intersect(you).Count()
    select count == 0 ? 0 : 1 << (count - 1);

var sum = points.Sum();
Debug.Assert(17782 == sum);
Console.WriteLine(sum);