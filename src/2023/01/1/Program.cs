using System.Text.RegularExpressions;

// Read the input text file,
// concat the first and last digit on each line
// parse number 
// then aggregate its sum.

const string pattern = @"(\d)";
const RegexOptions commonOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

var (first, last) = (
    new Regex(pattern, commonOptions), 
    new Regex(pattern, RegexOptions.RightToLeft | commonOptions));

var values = 
    from line in File.ReadAllLines(@"../input.txt")
    let digits = first.Match(line).Value + last.Match(line).Value
    select int.Parse(digits);

var sum = values.Aggregate((a, b) => a + b);      

Console.WriteLine(sum);
