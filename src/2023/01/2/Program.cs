// Build a pattern to match the first and last digit in humanoid or digit.
// Read the input text file,
// Find the first and last phrase on each line,
// map to index as literal character,
// concat the two digits and parse number 
// and aggregate its sum.

const RegexOptions commonOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

var map =
    Enumerable.Range(0, 10)
        .Select(i => i.ToString())
        .Concat(new[]
        {
            "zero", "one", "two", "three", "four", "five",
            "six", "seven", "eight", "nine"
        })
        .ToArray();

var pattern = "(" + string.Join('|', map) + ")";

var regexes = new[]
{
    new Regex(pattern, commonOptions),
    new Regex(pattern, RegexOptions.RightToLeft | commonOptions)
};

var values =
    from line in File.ReadAllLines(@"../input.txt")
    let phrases =
        from regex in regexes
        select regex.Match(line).Value
    let digits =
        from phrase in phrases
        select (char)('0' + Array.IndexOf(map, phrase) % 10)
    select int.Parse(new string(digits.ToArray()));

var sum = values.Aggregate((a, b) => a + b);

Debug.Assert(54078 == sum);
Console.WriteLine(sum);