using System.Buffers;
using System.Diagnostics;
using System.Text;

namespace Klinkby.AoC2025.Extensions;

/// <summary>
///     Zero allocation stream parser 
/// </summary>
internal static class StreamExtensions
{
    private readonly static Encoding FileEncoding = Encoding.Latin1;

    [DebuggerStepThrough]
    public static void ReadAllLines(this Stream stream, Action<ReadOnlySpan<char>> actionLine, int bufferSize = 256)
    {
        byte[] byteArray = ArrayPool<byte>.Shared.Rent(bufferSize);
        char[] charArray = ArrayPool<char>.Shared.Rent(bufferSize);

        try
        {
            ProcessStream(stream, byteArray, charArray, bufferSize, actionLine);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(byteArray);
            ArrayPool<char>.Shared.Return(charArray);
        }
    }

    private static void ProcessStream(Stream stream, byte[] byteArray, char[] charArray, int bufferSize, Action<ReadOnlySpan<char>> actionLine)
    {
        int lineLen = 0;

        while (true)
        {
            int read = stream.Read(byteArray, 0, bufferSize);
            if (read == 0)
            {
                FlushLine(charArray, ref lineLen, actionLine);
                break;
            }

            ProcessBuffer(byteArray.AsSpan(0, read), ref charArray, ref lineLen, actionLine);
        }
    }

    private static void ProcessBuffer(ReadOnlySpan<byte> buffer, ref char[] charArray, ref int lineLen, Action<ReadOnlySpan<char>> actionLine)
    {
        ReadOnlySpan<byte> remaining = buffer;

        while (remaining.Length > 0)
        {
            int nlIdx = remaining.IndexOf((byte)'\n');

            if (nlIdx < 0)
            {
                AppendToLine(remaining, ref charArray, ref lineLen);
                break;
            }

            ReadOnlySpan<byte> lineSegment = StripCarriageReturn(remaining[..nlIdx]);
            AppendAndEmitLine(lineSegment, ref charArray, ref lineLen, actionLine);
            remaining = remaining[(nlIdx + 1)..];
        }
    }

    private static void AppendAndEmitLine(ReadOnlySpan<byte> lineSegment, ref char[] charArray, ref int lineLen, Action<ReadOnlySpan<char>> actionLine)
    {
        AppendToLine(lineSegment, ref charArray, ref lineLen);
        FlushLine(charArray, ref lineLen, actionLine);
    }

    private static void AppendToLine(ReadOnlySpan<byte> segment, ref char[] charArray, ref int lineLen)
    {
        if (segment.Length == 0) return;

        EnsureCapacity(ref charArray, lineLen + segment.Length);
        lineLen += FileEncoding.GetChars(segment, charArray.AsSpan(lineLen));
    }

    private static void FlushLine(char[] charArray, ref int lineLen, Action<ReadOnlySpan<char>> actionLine)
    {
        if (lineLen > 0)
        {
            actionLine(charArray.AsSpan(0, lineLen));
            lineLen = 0;
        }
    }

    private static ReadOnlySpan<byte> StripCarriageReturn(ReadOnlySpan<byte> lineSegment)
    {
        return lineSegment.Length > 0 && lineSegment[^1] == (byte)'\r'
            ? lineSegment[..^1]
            : lineSegment;
    }

    private static void EnsureCapacity(ref char[] buffer, int requiredCapacity)
    {
        if (buffer.Length >= requiredCapacity) return;

        char[] newBuffer = ArrayPool<char>.Shared.Rent(Math.Max(requiredCapacity, buffer.Length * 2));
        buffer.AsSpan().CopyTo(newBuffer);
        ArrayPool<char>.Shared.Return(buffer);
        buffer = newBuffer;
    }
}