// Read the input text file,
// find the game id and data on each line
// extract the set of marble draws
// parse the count and color of the marbles
// detect impossible draws
// group by game id
// filter out any game that have an impossible draw
// aggregate the sum of the game ids.

var limit = new Dictionary<string, int>
    { { "red", 12 }, { "green", 13 }, { "blue", 14 } };
const RegexOptions commonOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

var gameRegex = new Regex(@"^Game (?'id'\d+): (?'data'.+)$", commonOptions);
var setRegex = new Regex(@"(?'set'.+?)(?:; |$)", commonOptions);
var marblesRegex = new Regex(@"(?'count'\d+) (?'color'\w+)(?:, |$)", commonOptions);

var gameIds =
    from line in File.ReadAllLines(@"../input.txt")
    let gameMatch = gameRegex.Match(line)
    let id = int.Parse(gameMatch.Groups["id"].Value)
    from setMatch in setRegex.Matches(gameMatch.Groups["data"].Value)
    let marbles =
        from marblesMatch in marblesRegex.Matches(setMatch.Groups["set"].Value)
        let count = int.Parse(marblesMatch.Groups["count"].Value)
        let color = marblesMatch.Groups["color"].Value
        select (color, count)
    let impossible = marbles.Any(m => m.count > limit[m.color])
    group impossible by id
    into g
    where g.All(x => !x)
    select g.Key;

var sum = gameIds.Sum();
Debug.Assert(2679 == sum);
Console.WriteLine(sum);