using System.Globalization;
using System.Text.RegularExpressions;

namespace Klinkby.AoC2024;

public partial class Day03
{
    private readonly string _input = EmbeddedResource.input_03_txt.ReadToEnd();

    [Test]
    public async Task Puzzle1()
    {
        int sum = ParseEnabledMuls(_input, forceEnable: true);
        await Assert.That(sum).IsEqualTo(171183089);
    }

    [Test]
    public async Task Puzzle2()
    {
        int sum = ParseEnabledMuls(_input);
        await Assert.That(sum).IsEqualTo(63866497);
    }

    private static int ParseEnabledMuls(ReadOnlySpan<char> text, bool forceEnable = false)
    {
        int sum = 0;
        bool enabled = true;
        foreach (ValueMatch token in EnablerMulFinder().EnumerateMatches(text))
        {
            ReadOnlySpan<char> value = text.Slice(token.Index, token.Length);
            switch (value)
            {
                case "do()":
                    enabled = true;
                    break;
                case "don't()":
                    enabled = false;
                    break;
                default:
                    if (enabled || forceEnable)
                    {
                        sum += ParseMul(value);
                    }
                    break;
            }
        }

        return sum;
    }

    private static int ParseMul(ReadOnlySpan<char> value)
    {
        int separatorIndex = value.IndexOf(',');
        return 
            int.Parse(value[4..separatorIndex], CultureInfo.InvariantCulture) *
            int.Parse(value[(separatorIndex + 1)..^1], CultureInfo.InvariantCulture);
    }
    
    [GeneratedRegex(@"do\(\)|don't\(\)|mul\(\d\d?\d?,\d\d?\d?\)",
        RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex EnablerMulFinder();
}