internal readonly record struct Hand(string Text, long Value, Result Rank)
{
    const string Cards = "23456789TJQKA";
    const string ResultCards = "J23456789TQKA";

    public static Hand From(string hand)
    {
        var value = ToValue(hand, Cards);
        var result = GetResult(ToValue(hand, ResultCards));
        return new Hand(hand, value, result);
    }

    private static long ToValue(string hand, string cards)
    {
        long value = 0;
        foreach (var c in hand)
        {
            value <<= 8;
            var index = cards.IndexOf(c);
            Debug.Assert(index >= 0);
            value |= (uint)cards.IndexOf(c);
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
        var jokersFound = found[0];
        found = found[1..];
        found.Sort();
        var rank = (found[^1] + jokersFound) switch
        {
            >=5 => Result.FiveOfAKind,
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