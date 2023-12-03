internal readonly record struct Token
{
    private const int Margin = 1;
    public readonly Rectangle Bounds;
    public readonly int Value;

    public Token(Match match, int y, bool parse = false)
    {
        Bounds = parse
            ? new Rectangle(match.Index, y, match.Length, 1)
            : new Rectangle(match.Index - Margin, y - Margin, 1 + Margin * 2, 1 + Margin * 2);
        if (parse) Value = int.Parse(match.Value);
    }

    public bool IsNumber => Value != 0;
}