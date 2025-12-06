namespace Klinkby.AoC2025;

/// <summary>
///     Day 6: Trash Compactor
/// </summary>
public sealed partial class Day06
{
    private delegate long Operator(long a, short b);

    [Theory]
    [InlineData(5733696195703)]
    public void Puzzle1(long expected)
    {
        using Stream stream = EmbeddedResource.day06_txt.GetStream();
        List<List<short>> rows = new(4);
        List<char> symbols = new(1000);
        stream.Read('\n', text => ParseLine(text, rows, symbols), 4000);

        long sum = symbols
            .AsValueEnumerable()
            .Select((symbol, index) =>
            {
                var (op, seed) = symbol == '+' ? ((Operator)Add, 0L) : (Mul, 1L);
                return rows.AsValueEnumerable().Aggregate(seed, (agg, cells) => op(agg, cells[index]));
            })
            .Sum();
        
        Assert.Equal(expected, sum);
    }
    
    [Theory]
    [InlineData(10951882745757)]
    public void Puzzle2(long expected)
    {
        using Stream stream = EmbeddedResource.day06_txt.GetStream();
        List<List<char>> columns = new(1000);
        
        stream.Read('\n', text => ReadColumns(text, columns), 3746);

        Operator op = Add;
        long problemResult = 0;
        long sum = columns.AsValueEnumerable().Aggregate(0L, (agg, column) => 
            ParseColumnValues(column, agg, ref problemResult, ref op)); 
        sum += problemResult; // last one

        Assert.Equal(expected, sum);   
    }

    private static long ParseColumnValues(List<char> column, long sum, ref long problemResult, ref Operator op)
    {
        if (!column.Exists(char.IsDigit)) // blank = section separator
        {
            sum += problemResult;
            return sum;
        }

        (op, problemResult) = column[^1] switch // change operator?
        {
            '+' => (Add, 0L),
            '*' => (Mul, 1L),
            _ => (op, problemResult)
        };
            
        if (CollectionsMarshal.AsSpan(column)[.. ^1].TryParseShort(out var value))
        {
            problemResult = op(problemResult, value); // add/mul value
        }

        return sum;
    }

    private static void ParseLine(ReadOnlySpan<char> text, List<List<short>> sheet, List<char> symbols)
    {
        List<short> row = new(1000);
        foreach (ValueMatch match in Digits().EnumerateMatches(text))
        {
            if (text[match.Index .. (match.Index + match.Length)].TryParseShort(out short value))
                row.Add(value);
        }

        if (row.Count != 0)
        {
            sheet.Add(row);
            return;
        }

        foreach (ValueMatch match in Operators().EnumerateMatches(text))
        {
            symbols.Add(text[match.Index]);
        }
    }

    private static void ReadColumns(ReadOnlySpan<char> text, List<List<char>> columns)
    {
        if (columns.Count == 0) // init, now we know the width
        {
            for (var i = text.Length - 1; i >= 0; i--)
            {
                columns.Add(new List<char>(5));
            }
        }

        for (var i = text.Length - 1; i >= 0; i--)
        {
            columns[i].Add(text[i]);
        }
    }

    private static long Add(long a, short b) => a + b;
    
    private static long Mul(long a, short b) => a * b;

    
    [GeneratedRegex(@"\d+", RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex Digits();
    
    [GeneratedRegex(@"\*|\+", RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex Operators();
}