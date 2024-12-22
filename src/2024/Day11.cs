namespace Klinkby.AoC2024;

public class Day11
{
    [Test]
    public async Task Puzzle1()
    {
        string stones = EmbeddedResource.day11_txt.ReadAllLines().First();
        for (int i = 0; i < 25; i++)
        {
            stones = stones
                .Split(" ")
                .Select(BlinkStone)
                .Aggregate("", static (a, b) => a + b)
                .TrimEnd();
        }

        long count = stones.Split(" ").Length;
        await Assert.That(count).IsEqualTo(217443);
    }

    private static string BlinkStone(string stone)
    {
        bool zero = stone is ['0'];
        bool evenDigits = stone.Length % 2 == 0;
        string result = (zero, evenDigits) switch
        {
            (true, _) => "1 ",
            (_, true) =>
                $"{long.Parse(stone[.. (stone.Length / 2)], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture)} {long.Parse(stone[(stone.Length / 2) ..], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture)} ",
            _ => $"{(long.Parse(stone, CultureInfo.InvariantCulture) * 2024).ToString(CultureInfo.InvariantCulture)} "
        };
        return result;
    }
}