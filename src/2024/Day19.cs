namespace Klinkby.AoC2024;

public class Day19
{
    private readonly static string[] Input = EmbeddedResource.day19_txt.ReadAllLines().ToArray();
    private readonly static string[] Towels = GetSortedTowels();
    private readonly static string[] Patterns = GetPatterns();

    [Test]
    public async Task Puzzle1()
    {
        int count = Patterns.Count(CombinesFromTowels);
        await Assert.That(count).IsEqualTo(315);
    }

    private static bool CombinesFromTowels(string pattern)
    {
        int i = Array.BinarySearch(Towels, pattern);
        if (i >= 0)
        {
            return true;
        }

        i = ~i;
        while (--i >= 0)
        {
            string towel = Towels[i];
            if (towel.Length > pattern.Length || towel != pattern[..towel.Length]) continue;
            if (towel[0] != pattern[0]) break;
            if (CombinesFromTowels(pattern[towel.Length..])) return true;
        }

        return false;
    }

    private static string[] GetPatterns()
    {
        return Input
            .SkipWhile(static x => x.Length > 0)
            .Skip(1)
            .ToArray();
    }
    
    private static string[] GetSortedTowels()
    {
        string[] towels = Input
            .TakeWhile(static x => x.Length != 0)
            .Aggregate("", static (acc, x) => acc + x + ", ")
            .Split(", ", StringSplitOptions.RemoveEmptyEntries);
        Array.Sort(towels);
        return towels;
    }
}