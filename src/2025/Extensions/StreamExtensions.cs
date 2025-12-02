using System.Diagnostics;
using System.Text;

namespace Klinkby.AoC2025.Extensions;

[DebuggerStepThrough]
internal static class StreamExtensions
{
    private readonly static Encoding FileEncoding = Encoding.Latin1;

    extension(Stream stream)
    {
        public long ReadAggregate(char splitter, Func<ReadOnlySpan<char>, long> aggregateFunc, int bufferSize = 100)
        {
            long sum = 0L;
            stream.Read(splitter, text => sum += aggregateFunc(text), bufferSize);
            return sum;
        }

        public void Read(char splitter, Action<ReadOnlySpan<char>> use, int bufferSize = 100)
        {
            Span<char> buffer = stackalloc char[FileEncoding.GetMaxByteCount(bufferSize)];
            using StreamReader sr = new(stream, FileEncoding);
            int read;
            int i = 0;
            while ((read = sr.Read()) != -1)
            {
                char ch = (char)read;
                if (ch == splitter && i != 0)
                {
                    use(buffer[.. i]);
                    i = 0;
                    continue;
                }

                buffer[i++] = ch;
                ArgumentOutOfRangeException.ThrowIfEqual(bufferSize, i);
            }

            if (i != 0)
            {
                use(buffer[.. i]);
            }
        }
    }
}