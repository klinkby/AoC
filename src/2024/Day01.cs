namespace Klinkby.AoC2024;

public partial class Day01
{
    [Test]
    public async Task Puzzle1()
    {
        int[][] columns = ReadInputAsColumns();
        int rows = columns[0].Length;
        
        Array.Sort(columns[0]);
        Array.Sort(columns[1]);

        long sum = Enumerable
            .Range(0, rows)
            .Sum(i => Math.Abs(columns[0][i] - columns[1][i]));

        await Assert.That(sum).IsEqualTo(2970687);
    }
    
    [Test]
    public async Task Puzzle2()
    {
        int[][] columns = ReadInputAsColumns();
        int rows = columns[0].Length;
        int[] score = new int[rows];
        Span<int> column1 = columns[1];
        for (int i = 0; i < rows; i++)
        {
            int value = columns[0][i];
            score[i] = value * column1.Count(value);
        }

        int sum = score.Sum();
        await Assert.That(sum).IsEqualTo(23963899);
    }
    
    private static int[][] ReadInputAsColumns()
    {
        return EmbeddedResource.input_01_txt
            .ReadAllLines()
            .ParseInts(SpaceSplitter())
            .AsColumns(2);
    }

    [GeneratedRegex(@"\s+", RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex SpaceSplitter();    
}