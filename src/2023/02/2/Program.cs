// Read the input text file,
// find the game data on each line
// extract the set of marble draws
// parse the count and color of the marbles
// group by color and multiply their max count
// aggregate the sum.

const RegexOptions commonOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

var gameRegex = new Regex(@"^Game (?'id'\d+): (?'data'.+)$", commonOptions);
var setRegex = new Regex(@"(?'set'.+?)(?:; |$)", commonOptions);
var marblesRegex = new Regex(@"(?'count'\d+) (?'color'\w+)(?:, |$)", commonOptions);

var powers =
    from line in File.ReadAllLines(@"../input.txt")
    let game =
        from setMatch in setRegex.Matches(gameRegex.Match(line).Groups["data"].Value)
        from marblesMatch in marblesRegex.Matches(setMatch.Groups["set"].Value)
        let color = marblesMatch.Groups["color"].Value
        let count = int.Parse(marblesMatch.Groups["count"].Value)
        select (color, count)
    select game.GroupBy(x => x.color, x => x.count)
        .Select(x => x.Max())
        .Aggregate(1, (a, b) => a * b);

var sum = powers.Sum();
Debug.Assert(77607 == sum);
Console.WriteLine(sum);