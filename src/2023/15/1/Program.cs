// Read the input text file and parse sequences.
// Summarize the aggregated ASCII values * 17 % 256. 

var reg = new Regex("(?'seq'[^,]+)(?:,|$)", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
var input = File.ReadLines("../input.txt").First();
var sum = (
    from m in reg.Matches(input)
    select m.Groups["seq"].Value.Aggregate(0L, (a, b) => (a + b) * 17 % 256)
).Sum();

Debug.Assert(510792 == sum);
Console.WriteLine(sum);