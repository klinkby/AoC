// Read the input text file,
// Parse the lines into winning numbers and your numbers,
// Intersect the two sets of numbers to find the wins on each card.
// Iterate the cards, increment the following (wins) cards, 
// then summarize.

var regex = new Regex(@"(?::)(?:\s+(?'win'\d+))+(?:\s\|)(?:\s+(?'you'\d+))+$",
    RegexOptions.Compiled | RegexOptions.ExplicitCapture);

var wins = (
    from line in File.ReadAllLines(@"../input.txt")
    from m in regex.Matches(line)
    let win = m.Groups["win"].Captures.Select(c => int.Parse(c.Value))
    let you = m.Groups["you"].Captures.Select(c => int.Parse(c.Value))
    select win.Intersect(you).Count()
).ToArray();

var cards = new int[wins.Length];
Array.Fill(cards, 1);
for (var i = 0; i < cards.Length; i++)
for (var j = Math.Min(i + wins[i], cards.Length); j > i; j--)
    cards[j] += cards[i];

var sum = cards.Sum();
Debug.Assert(8477787 == sum);
Console.WriteLine(sum);