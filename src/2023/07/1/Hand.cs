internal readonly record struct Hand(long Value, Result Rank)
{
    const string Cards = "23456789TJQKA";

    public static Hand From(string hand)
    {
        var value = ToValue(hand);
        var result = GetResult(value);
        return new Hand(value, result);
    }

    private static long ToValue(string hand)
    {
        long value = 0;
        foreach (var c in hand)
        {
            value <<= 8;
            value |= (uint)Cards.IndexOf(c);
        }

        return value;
    }

    private static Result GetResult(long handValue)
    {
        Span<int> found = stackalloc int[Cards.Length];
        for (var i = 0; i < 5; i++)
        {
            found[(int)(handValue & 0xf)]++;
            handValue >>= 8;
        }

        found.Sort();
        var rank = found[^1] switch
        {
            5 => Result.FiveOfAKind,
            4 => Result.FourOfAKind,
            3 => found[^2] switch
            {
                2 => Result.FullHouse,
                _ => Result.ThreeOfAKind
            },
            2 => found[^2] switch
            {
                2 => Result.TwoPair,
                _ => Result.OnePair
            },
            _ => Result.HighCard
        };
        return rank;
    }
}