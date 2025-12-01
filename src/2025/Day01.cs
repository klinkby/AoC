namespace Klinkby.AoC2025;

/// <summary>
///     Day 1: Secret Entrance
/// </summary>
public sealed class Day01
{
    const long StartPosition = 50;
    const long DialSteps = 100;

    [Theory, InlineData(1021)]
    public void Puzzle1(long expected)
    {
        const long CountPosition = 0;

        long password = 0;
        long position = StartPosition;
        
        using Stream stream = EmbeddedResource.day01_txt.GetStream();
        stream.ReadAllLines(line =>
        {
            int value = ParseValue(line);
            position = (position + value) % DialSteps;
            if (CountPosition == position) password++;
        });
        
        Assert.Equal(expected, password);
    }
    
    [Theory, InlineData(5933)]
    public void Puzzle2(long expected)
    {
        long password = 0;
        long dialPosition = StartPosition;
        
        using Stream stream = EmbeddedResource.day01_txt.GetStream();
        stream.ReadAllLines(line =>
        {
            int value = ParseValue(line);
            bool wasZero = dialPosition == 0;
            
            long newPosition = dialPosition + value;
            long nextPosition = Math.DivRem(newPosition, DialSteps, out dialPosition /* = remainder */);
            
            password += (wasZero, newPosition) switch
            {
                (true, _) => Math.Abs(newPosition) / DialSteps,
                (_, 0) => 1,
                (_, > 0) => nextPosition,
                (_, < 0) => 1 + Math.Abs(newPosition) / DialSteps
            };
            
            if (dialPosition < 0) dialPosition += DialSteps;
        });
        
        Assert.Equal(expected, password);
    }

    private static int ParseValue(ReadOnlySpan<char> line)
    {
        int value = int.Parse(line[1..], CultureInfo.InvariantCulture);
        return line[0] == 'L' ? -value : value;
    }
}