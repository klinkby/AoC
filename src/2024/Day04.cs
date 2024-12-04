namespace Klinkby.AoC2024;

public partial class Day04
{
    [Test]
    public async Task Puzzle1()
    {
        IReadOnlyList<IReadOnlyList<char>> horizontal = EmbeddedResource.input_04_txt.ReadCharMatrix();
        IReadOnlyList<IReadOnlyList<char>> vertical = Enumerable.Range(0, horizontal[0].Count)
            .Select(x => Enumerable.Range(0, horizontal.Count)
                .Select(y => horizontal[y][x])
                .ToArray())
            .ToArray();
        int sum = new[]
            {
                StreamRectangle(horizontal),
                StreamRectangle(vertical),
                StreamRhumbus(horizontal),
                StreamRhumbus(horizontal.Reverse().ToArray())
            }
            .Select(stream => stream.ToArray())
            .Sum(text => XmasFinder().Count(text) + SamxFinder().Count(text));

        await Assert.That(sum).IsEqualTo(2551);
    }

    private static IEnumerable<char> StreamRectangle(IReadOnlyList<IReadOnlyList<char>> m) =>
        m.SelectMany(line => line.Concat("\n"));

    private static IEnumerable<char> StreamRhumbus(IReadOnlyList<IReadOnlyList<char>> m) =>
        Enumerable.Range(-m[0].Count, 2 * m[0].Count)
            .SelectMany(x =>
                m.Select((line, y) =>
                    x + y >= 0 && x + y < m[0].Count ? line[x + y] : ' ').Concat("\n"));

    [GeneratedRegex("XMAS",
        RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex XmasFinder();

    [GeneratedRegex("SAMX",
        RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex SamxFinder();
}