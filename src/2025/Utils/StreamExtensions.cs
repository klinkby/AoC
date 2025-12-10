using System.Diagnostics;
using System.Text;

namespace Klinkby.AoC2025.Utils;

[DebuggerStepThrough]
internal static class StreamExtensions
{
    private const int DefaultBufferSize = 250;
    private static readonly Encoding FileEncoding = Encoding.Latin1;

    extension(Stream stream)
    {
        public long ReadAggregate(char splitter, Func<ReadOnlySpan<char>, long> aggregateFunc, int bufferSize = DefaultBufferSize)
        {
            long sum = 0L;
            stream.Read(splitter, text => sum += aggregateFunc(text), bufferSize);
            return sum;
        }

        public long ReadAggregate<T>(char splitter, T state, Func<ReadOnlySpan<char>, T, long> aggregateFunc, int bufferSize = DefaultBufferSize) where T: allows ref struct
        {
            long sum = 0L;
            stream.Read(splitter, state, (text, innerState) => sum += aggregateFunc(text, innerState), bufferSize);
            return sum;
        }
        
        public void Read(char splitter, Action<ReadOnlySpan<char>> use, int bufferSize = DefaultBufferSize) => 
            stream.Read<object?>(splitter, null, (text, _) => use(text), bufferSize);
        
        public void Read<T>(char splitter, T state, Action<ReadOnlySpan<char>, T> use, int bufferSize = DefaultBufferSize) where T: allows ref struct
        {
            Span<char> buffer = stackalloc char[FileEncoding.GetMaxByteCount(bufferSize)];
            using StreamReader sr = new(stream, FileEncoding);
            int read;
            int i = 0;
            while ((read = sr.Read()) != -1)
            {
                char ch = (char)read;
                if (ch == '\r') continue;
                if (ch == splitter)
                {
                    use(buffer[.. i], state);
                    i = 0;
                    continue;
                }

                buffer[i++] = ch;
                ArgumentOutOfRangeException.ThrowIfEqual(bufferSize, i);
            }

            if (i != 0)
            {
                use(buffer[.. i], state);
            }
        }
    }
}