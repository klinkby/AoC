namespace Klinkby.AoC2024;

public class Day09
{
    private const char ZeroAscii = '0';
    private const ushort Free = ushort.MaxValue;

    [Test]
    public async Task Puzzle1()
    {
        Span<ushort> blocks = GetBlocks();
        Defragment(blocks);

        long sum = CalculateChecksum(blocks);
        await Assert.That(sum).IsEqualTo(6367087064415);
    }

    private static Span<ushort> GetBlocks()
    {
        IEnumerable<char> DenseDisk = EmbeddedResource.day09_txt
            .ReadAllLines()
            .First();
        Span<ushort> blocks = DenseDisk
            .Select(static (ch, i) => Enumerable.Repeat((ushort)(i % 2 == 0 ? i / 2 : Free), ch - ZeroAscii))
            .SelectMany(static x => x)
            .ToArray();
        return blocks;
    }

    private static void Defragment(Span<ushort> blocks)
    {
        int freeIndex = 0;
        for (int fileIndex = blocks.Length - 1; fileIndex >= 0; fileIndex--)
        {
            if (blocks[fileIndex] == Free)
            {
                continue;
            }

            freeIndex += blocks[freeIndex..].IndexOf(Free);
            if (freeIndex < 0 || freeIndex >= fileIndex)
            {
                break;
            }

            blocks[freeIndex] = blocks[fileIndex];
            blocks[fileIndex] = Free;
            freeIndex++;
        }
    }
    
    private static long CalculateChecksum(ReadOnlySpan<ushort> blocks)
    {
        long sum = 0;
        for (int i = 0; i < blocks.Length; i++)
        {
            sum += blocks[i] == Free ? 0 : blocks[i] * i;
        }

        return sum;
    }
}