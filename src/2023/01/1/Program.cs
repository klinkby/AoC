// Read the input text file,
// concat the first and last digit on each line
// parse number 
// then aggregate its sum.

const string pattern = @"(\d)";
const RegexOptions commonOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

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
    select int.Parse(string.Concat(phrases));

var sum = values.Aggregate((a, b) => a + b);

Debug.Assert(54601 == sum);
Console.WriteLine(sum);