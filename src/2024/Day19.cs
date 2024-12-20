namespace Klinkby.AoC2024;

public partial class Day19
{
    private readonly static string[] Input = EmbeddedResource.input_day19_txt.ReadAllLines().ToArray();
    private readonly static string[] Towels = GetSortedTowels();

    [Test]
    public async Task Puzzle1()
    {
        var patterns = Input
            .SkipWhile(x => x.Length > 0)
            .Skip(1);
        int count = patterns.Count(CombinesFromTowels);
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
            if (towel.Length < pattern.Length &&
                towel == pattern[..towel.Length]
                && CombinesFromTowels(pattern[towel.Length..]))
            {
                return true;
            }
        }

        return false;
    }

    private static string[] GetSortedTowels()
    {
        string[] towels = Input
            .TakeWhile(x => x.Length != 0)
            .Aggregate("", (acc, x) => acc + x + ", ")
            .Split(", ", StringSplitOptions.RemoveEmptyEntries);
        Array.Sort(towels);
        return towels;
    }
}