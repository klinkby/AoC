using System.Globalization;
using System.Text.RegularExpressions;

namespace Solve.AoC2023;

/// <summary>
///     Read the input text file,
///     find the first and last digit on each line
///     concatenate and parse number
///     then aggregate its sum.
/// </summary>
public sealed class Day01Puzzle1 : IPuzzleSolver
{
    private const string Pattern = @"(\d)";
    private const RegexOptions CommonOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

    public string Name => "Trebuchet?!";

    public Task<string> RunAsync(IList<string> input, IProgress<double> progress, CancellationToken cancellationToken)
    {
        Regex[] regexes =
        [
            new Regex(Pattern, CommonOptions), new Regex(Pattern, RegexOptions.RightToLeft | CommonOptions)
        ];

        IEnumerable<int> values =
            from line in input
            let digits =
                from regex in regexes
                select regex.Match(line).Value
            select int.Parse(string.Concat(digits), CultureInfo.CurrentCulture);

        return Task.FromResult(values.Sum().ToString(CultureInfo.CurrentCulture));
    }
}