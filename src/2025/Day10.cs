namespace Klinkby.AoC2025;

/// <summary>
///     Day 10: Factory
/// </summary>
public sealed partial class Day10
{
    private const int Capacity = 25;
    
    [Theory]
    [InlineData(417)]
    public void Puzzle1(long expected)
    {
        using Stream stream = EmbeddedResource.day10_txt.GetStream();
        Span<uint> setBuffer = stackalloc uint[Capacity];
        Queue<(uint state, int presses)> queue = new(Capacity);
        HashSet<uint> visited = new (Capacity);
        long sum = stream.ReadAggregate('\n', setBuffer, (text, sets) =>
        {
            Reset();
            Parse(text, out uint desired, sets, out int setCount);
            return FindMinimumIterations(sets[ .. setCount], desired);
        });

        Assert.Equal(expected, sum);
        return;

        long FindMinimumIterations(ReadOnlySpan<uint> sets, uint desired)
        {
            var minIterations = long.MaxValue;
            queue.Enqueue((0, 0));
            visited.Add(0);

            while (queue.Count > 0)
            {
                var (currentState, iterations) = queue.Dequeue();
                if (currentState == desired)
                {
                    minIterations = iterations;
                    break;
                }

                for (var i = sets.Length - 1; i >= 0; i--)
                {
                    var nextState = currentState ^ sets[i];
                    if (visited.Add(nextState)) queue.Enqueue((nextState, iterations + 1));
                }
            }

            return minIterations;
        }

        void Reset()
        {
            queue.Clear();
            visited.Clear();
        }
    }

    private static void Parse(ReadOnlySpan<char> text, out uint desired, Span<uint> buttonSets, out int buttonSetCount)
    {
        int count = text.IndexOf(']') - 1;
        desired = 0;
        for (int i = 1; i <= count; i++)
        {
            desired = desired << 1 | (text[i] == '#' ? 1u : 0);
        }
        
        var wiringSpan = text[(count + 2)..];
        buttonSetCount = 0;
        foreach (var group in GroupParser().EnumerateMatches(wiringSpan))
        {
            uint buttonSet = 0;
            var groupSpan = wiringSpan[group.Index..(group.Index + group.Length)];
            foreach (var digits in WiringParser().EnumerateMatches(groupSpan))
            {
                var digitsSpan = groupSpan[digits.Index .. (digits.Index + digits.Length)];
                int value = int.Parse(digitsSpan, CultureInfo.InvariantCulture);
                buttonSet |= 1u << (count - 1 - value);
            }
            
            buttonSets[buttonSetCount] = buttonSet;
            buttonSetCount++;
        }
    }

    [GeneratedRegex(@"\(([^\)]+)", RegexOptions.CultureInvariant | RegexOptions.NonBacktracking | RegexOptions.ExplicitCapture)]
    private static partial Regex GroupParser();

    [GeneratedRegex(@"(\d+)", RegexOptions.CultureInvariant | RegexOptions.NonBacktracking | RegexOptions.ExplicitCapture)]
    private static partial Regex WiringParser();
}
