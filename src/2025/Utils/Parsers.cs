using System.Diagnostics;

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
    
    public static bool TryParseTuple(
        this ReadOnlySpan<char> text,
        out ValueTuple<int, int> value,
        char separator = ',')
    {
        int split = text.IndexOf(separator);
        if (split >= 0
            && TryParseInt(text[.. split], out var item1)
            && TryParseInt(text[(split + 1) ..], out var item2))
        {
            value = new(item1, item2);
            return true;
        }

        value = default;
        return false;
    }
}