namespace Klinkby.AoC2024;

public partial class Day05
{
    [Test]
    public async Task Puzzle1()
    {
        Dictionary<int, List<int>> ruleMap = GetRules();
        int sum = GetUpdates()
            .Where(update => Validate(update, ruleMap))
            .Sum(GetMedian);

        await Assert.That(sum).IsEqualTo(4662);
    }
    
    [Test]
    public async Task Puzzle2()
    {
        Dictionary<int, List<int>> ruleMap = GetRules();
        PageComparer comparer = new(ruleMap);
        int sum = GetUpdates()
            .Where(update => !Validate(update, ruleMap))
            .Select(update => update.Order(comparer).ToArray())
            .Sum(GetMedian);

        await Assert.That(sum).IsEqualTo(5900);
    }

    private static IEnumerable<List<int>> GetUpdates()
    {
        return EmbeddedResource.day05_txt.ReadAllLines()
            .SkipWhile(x => x.Contains('|', StringComparison.Ordinal))
            .ParseInts(Splitter());
    }

    private static Dictionary<int, List<int>> GetRules()
    {
        IEnumerable<string> input = EmbeddedResource.day05_txt.ReadAllLines();
        Dictionary<int, List<int>> ruleMap = [];
        IEnumerable<List<int>> rules = input
            .TakeWhile(x => x.Contains('|', StringComparison.Ordinal))
            .ParseInts(Splitter());
        foreach (List<int> rule in rules)
        {
            if (!ruleMap.TryGetValue(rule[1], out List<int>? after))
            {
                after = [];
                ruleMap.Add(rule[1], after);
            }

            after.Add(rule[0]);
        }

        return ruleMap;
    }

    private static bool Validate(List<int> pages, Dictionary<int, List<int>> ruleMap)
    {
        List<int>? rules = null;
        IEnumerable<bool> pageOk =
            from i in Enumerable.Range(0, pages.Count)
            let page = pages[i]
            let pagesAfter = pages[(i + 1)..]
            where ruleMap.TryGetValue(page, out rules)
            select pagesAfter.Intersect(rules).Any();

        return pageOk.All(x => !x);
    }

    private static int GetMedian(IReadOnlyList<int> pages)
    {
        return pages[pages.Count / 2];
    }

    [GeneratedRegex(@",|\|",
        RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex Splitter();
    
    private sealed class PageComparer(Dictionary<int, List<int>> ruleMap) : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return Validate([x, y], ruleMap) ? 1 : -1;
        }
    }
}