namespace Klinkby.AoC2025.Models;

public readonly record struct LongRange(long From,  long To)
{
    public long From { get; } = From;
    public long To { get; } = To;
    
    public bool Contains(long value) => value >= From && value <= To;

    public bool Overlaps(LongRange value) =>
        value.From <= To && value.To >= From;

}

public sealed class LongRangeComparer : IComparer<LongRange>
{
    public static LongRangeComparer Instance { get; } = new();
    
    public int Compare(LongRange x, LongRange y)
    {
        int fromComparison = x.From.CompareTo(y.From);
        return fromComparison != 0 ? fromComparison : x.To.CompareTo(y.To);
    }
}