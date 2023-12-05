// Read the input text file,
// Parse the lines into groups of numbers (0 is the seeds).
// Get the seeds (group 0) and the maps (groups 1..n).
// Then run each seed through the maps, and find the minimum location. 

using Klinkby.Toolkitt;

var regex = new Regex(@"(?'map'map\:)|(?'num'\d+)",
    RegexOptions.Compiled | RegexOptions.ExplicitCapture);

var groupId = 0;
var data = (
    from line in File.ReadAllLines(@"../input.txt")
    let m = regex.Match(line)
    let map = m.Groups["map"].Success
    let num = m.Groups["num"].Success
        ? regex.Matches(line)
            .SelectMany(m => m.Groups["num"].Captures.Select(c => long.Parse(c.Value)))
        : Array.Empty<long>()
    let id = map ? ++groupId : groupId
    select (id, num)
).ToArray();

var seeds = data
    .TakeWhile(x => 0 == x.id)
    .SelectMany(x => x.num);

var parsed = (destination: 0L, source: 0L, length: 0L);
var maps = data
    .SkipWhile(x => 0 == x.id)
    .Where(x => x.num.TryToValueTuple(out parsed))
    .GroupBy(x => x.id, x => parsed);

var locations = seeds.Select(seed =>
    maps.Aggregate(seed, (acc, mapGroup) =>
    {
        var skipRest = false;
        return mapGroup.Aggregate(acc, (value, map) =>
        {
            if (skipRest || value < map.source || value > map.source + map.length) return value;
            value += map.destination - map.source;
            skipRest = true;
            return value;
        });
    }));

var closest = locations.Min();
Debug.Assert(175622908 == closest);
Console.WriteLine(closest);