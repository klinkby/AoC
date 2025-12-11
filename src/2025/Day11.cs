using System.Text;

namespace Klinkby.AoC2025;

/// <summary>
///     Day 11: Reactor
/// </summary>
public sealed class Day11
{
    private static readonly Encoding FileEncoding = Encoding.Latin1;
    private static readonly uint You = ParseDevice("you");
    private static readonly uint Out = ParseDevice("out");
    
    [Theory]
    [InlineData(566)]
    public void Puzzle1(long expected)
    {
        using Stream stream = EmbeddedResource.day11_txt.GetStream();
        SortedDictionary<uint, uint[]> tree = new();
        stream.Read('\n', text =>
        {
            Parse(text, out uint device, out uint[] outputs);
            tree.Add(device, outputs);
        });

        long sum = VisitTree(tree);

        Assert.Equal(expected, sum);
    }

    private static long VisitTree(SortedDictionary<uint, uint[]> tree)
    {
        long sum = 0L;
        Queue<uint> stack = new([You]);
        HashSet<uint> visited = new([You]); 
        while (stack.TryDequeue(out uint device))
        {
            if (device == Out)
            {
                sum++; // found the exit
                continue;
            }

            foreach (uint output in tree[device])
            {
                if (visited.Contains(output)) continue; // loop detected
                stack.Enqueue(output);
            }
        }

        return sum;
    }

    private static void Parse(ReadOnlySpan<char> text, out uint device, out uint[] outputs)
    {
        device = ParseDevice(text[..3]);
        int count = (text.Length - 4) / 4;
        outputs = new uint[count];
        for (int i = text.Length - 3; i > 3; i-=4)
        {
            outputs[--count] = ParseDevice(text[i..(i + 3)]);
        }
    }

    private static uint ParseDevice(ReadOnlySpan<char> text)
    {
        Span<byte> intBuffer = stackalloc byte[4];
        _ = FileEncoding.GetBytes(text, intBuffer);
        return BitConverter.ToUInt32(intBuffer);
    }
}