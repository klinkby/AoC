namespace Klinkby.AoC2024;

public class Day01
{
    private readonly IReadOnlyList<IReadOnlyList<int>> _input = EmbeddedResource.input_01_txt.ReadAllIntegers();

    [Test]
    public async Task Puzzle1()
    {
        int[][] columns = _input.AsColumns(2);
        
        Array.Sort(columns[0]);
        Array.Sort(columns[1]);

        long sum = Enumerable
            .Range(0, _input.Count)
            .Sum(i => Math.Abs(columns[0][i] - columns[1][i]));

        await Assert.That(sum).IsEqualTo(2970687);
    }

    [Test]
    public async Task Puzzle2()
    {
        int[][] columns = _input.AsColumns(2);
        int count = _input.Count;
        int[] score = new int[count];
        Span<int> column1 = columns[1];
        for (int i = 0; i < count; i++)
        {
            int value = columns[0][i];
            score[i] = value * column1.Count(value);
        }

        int sum = score.Sum();
        await Assert.That(sum).IsEqualTo(23963899);
    }
}