// Read the input text file,
// find the first and last digit on each line
// concatenate and parse number 
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
    let digits =
        from regex in regexes
        select regex.Match(line).Value
    select int.Parse(string.Concat(digits));

var sum = values.Sum();
Debug.Assert(54601 == sum);
Console.WriteLine(sum);