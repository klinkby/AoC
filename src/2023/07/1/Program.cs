// Read the input text file, parse hand and bid.
// Order by result, value.
// Multiply bid by rank, then summarize.

var regex = new Regex(@"(?'hand'[\w\d]+)\s+(?'bid'\d+)", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

var i = 1;
var winnings = (
    from line in File.ReadAllLines(@"..\..\..\..\input.txt")
    let match = regex.Match(line)
    let hand = Hand.From(match.Groups["hand"].Value)
    let bid = int.Parse(match.Groups["bid"].Value)
    select (hand, bid)
).OrderBy(x => x.hand, Comparer<Hand>.Create((a, b) =>
{
    var c = a.Rank.CompareTo(b.Rank);
    return 0 == c ? a.Value.CompareTo(b.Value) : c;
})).Aggregate(0, (a, b) => a + b.bid * i++);

Debug.Assert(251545216 == winnings);
Console.WriteLine(winnings);