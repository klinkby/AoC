namespace Klinkby.AoC2025;

/// <summary>
///     Day 10: Factory
/// </summary>
public sealed partial class Day10
{
    [Theory]
    [InlineData(417)]
    public void Puzzle1(long expected)
    {
        using Stream stream = EmbeddedResource.day10_txt.GetStream();
        Span<uint> setBuffer = stackalloc uint[20];
        long sum = stream.ReadAggregate('\n', setBuffer, (text, sets) =>
        {
            Parse(text, out uint desired, sets, out int setCount);
            long leastApplications = long.MaxValue;

            // Breadth-first search (BFS) to find minimum presses
            var queue = new Queue<(uint state, int presses)>([(0, 0)]);
            var visited = new HashSet<uint> { 0 };
            while (queue.Count > 0)
            {
                var (currentState, presses) = queue.Dequeue();
                if (currentState == desired)
                {
                    leastApplications = presses;
                    break;
                }

                for (int i = 0; i < setCount; i++)
                {
                    uint nextState = currentState ^ sets[i];
                    if (visited.Add(nextState))
                    {
                        queue.Enqueue((nextState, presses + 1));
                    }
                }
            }

            return leastApplications;
        }, 250);

        Assert.Equal(expected, sum);
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

    [GeneratedRegex(@"\(([^\)]+)", RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex GroupParser();

    [GeneratedRegex(@"(\d+)", RegexOptions.CultureInvariant | RegexOptions.NonBacktracking | RegexOptions.ExplicitCapture)]
    private static partial Regex WiringParser();
}
