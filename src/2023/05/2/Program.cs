// Read the input text file,
// Parse the lines into groups of numbers (0 is the seeds).
// Get the seed ranges (group 0) and the maps (groups 1..n).
// Partition the seed ranges over available cores, and enumerate brute force
// puzzle 1 solution to find the minimum location. 
// OMG feel like running a bitcoin mining rig here.

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

var seedRanges = data
    .TakeWhile(x => 0 == x.id)
    .SelectMany(x => x.num)
    .Chunk(2)
    .Select(x => (start: x[0], count: x[1]))
    .ToArray();

var parsed = (destination: 0L, source: 0L, length: 0L);
var maps = data
    .SkipWhile(x => 0 == x.id)
    .Where(x => x.num.TryToValueTuple(out parsed))
    .GroupBy(x => x.id, x => parsed)
    .ToArray();

var sync = new object();
var closest = long.MaxValue;
Parallel.ForEach(seedRanges, seedRange =>
{
    var rangeMin = long.MaxValue;
    foreach (var seed in LongRange(seedRange.start, seedRange.count))
    {
        var location = maps.Aggregate(seed, (acc, mapGroup) =>
        {
            var skipRest = false;
            return mapGroup.Aggregate(acc, (value, map) =>
            {
                if (skipRest || value < map.source || value > map.source + map.length) return value;
                value += map.destination - map.source;
                skipRest = true;
                return value;
            });
        });
        if (location < rangeMin) rangeMin = location;
    }
    lock (sync)
    {
        if (rangeMin < closest) closest = rangeMin;
    }
});

Debug.Assert(5200543 == closest);
Console.WriteLine(closest);

static IEnumerable<long> LongRange(long start, long count)
{
    for (var i = 0; i < count; i++) yield return start + i;
}