using System.Diagnostics;
using System.Runtime.Intrinsics;

namespace Klinkby.AoC2025.Utils;

[DebuggerStepThrough]
internal static class Parsers
{ 
    public static bool TryParseLong(this ReadOnlySpan<char> text, out long value) => 
        long.TryParse(text, CultureInfo.InvariantCulture, out value);

    public static bool TryParseInt(this ReadOnlySpan<char> text, out int value) => 
        int.TryParse(text, CultureInfo.InvariantCulture, out value);

    public static bool TryParseShort(this ReadOnlySpan<char> text, out short value) => 
        short.TryParse(text, CultureInfo.InvariantCulture, out value);

    public static bool TryParseRange(
        this ReadOnlySpan<char> text,
        out LongRange range)
    {
        long from, to;
        int split = text.IndexOf('-');
        if (split >= 0
            && TryParseLong(text[.. split], out from)
            && TryParseLong(text[(split + 1) ..], out to))
        {
            range = new(from, to);
            return true;
        }

        range = default;
        return false;
    }
    
    public static bool TryParseVector(
        this ReadOnlySpan<char> text,
        out Vector64<int> vector,
        char separator = ',')
    {
        int split = text.IndexOf(separator);
        
        Span<int> values = stackalloc int[2];
        if (split >= 0
            && TryParseInt(text[.. split], out values[0])
            && TryParseInt(text[(split + 1) ..], out values[1]))
        {
            vector = Vector64.Create(values);
            return true;
        }

        vector = default;
        return false;
    }
}