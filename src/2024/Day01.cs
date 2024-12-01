using Klinkby.AoC2024.Extensions;

namespace Klinkby.AoC2024;

public class Day01
{
    private static ReadOnlySpan<string> Input => EmbeddedResource.input_01_txt.ReadAllLines();
    private readonly int[][] _columns = Input.ParseIntegerColumns(2);

    [Test]
    public async Task Puzzle1()
    {
        Array.Sort(_columns[0]);
        Array.Sort(_columns[1]);

        long sum = Enumerable
            .Range(0, Input.Length)
            .Sum(i => Math.Abs(_columns[0][i] - _columns[1][i]));

        await Assert.That(sum).IsEqualTo(2970687);
    }

    [Test]
    public async Task Puzzle2()
    {
        int count = Input.Length;
        int[] score = new int[count];
        Span<int> column1 = _columns[1];
        for (int i = 0; i < count; i++)
        {
            int value = _columns[0][i];
            score[i] = value * column1.Count(value);
        }
        int sum = score.Sum();
        await Assert.That(sum).IsEqualTo(23963899);
    }
}