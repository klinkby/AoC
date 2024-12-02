namespace Klinkby.AoC2024;

public class Day02
{
    private readonly IReadOnlyList<IReadOnlyList<int>> _input = EmbeddedResource.input_02_txt.ReadAllIntegers();

    [Test]
    public async Task Puzzle1()
    {
        int sum = _input.Sum(static levels => IsSafe(levels) ? 1 : 0);

        await Assert.That(sum).IsEqualTo(269);
    }

    [Test]
    public async Task Puzzle2()
    {
        int sum = _input
            .Select(MissingOneElement)
            .Count(static levels => levels.Any(IsSafe));

        await Assert.That(sum).IsEqualTo(337);
    }

    private static IEnumerable<int[]> MissingOneElement(IReadOnlyCollection<int> collection)
    {
        return collection
            .Select((_, i) => collection
                .Where((_, j) => j != i)
                .ToArray());
    }

    private static bool IsSafe(IReadOnlyList<int> levels)
    {
        Func<int, int, int> differ = levels[1] - levels[0] < 0
            ? static (a, b) => b - a
            : static (a, b) => a - b;
        for (int i = 1; i < levels.Count; i++)
        {
            if (differ(levels[i], levels[i - 1]) switch
                {
                    >= 1 and <= 3 => false,
                    _ => true
                })
            {
                return false;
            }
        }

        return true;
    }
}