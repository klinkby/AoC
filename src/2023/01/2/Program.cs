using System.Text.RegularExpressions;

/// Build a pattern to match the first and last digit in humanoid or digit.
/// Read the input text file,
/// Find the first and last phrase on each line,
/// map to index as literal character,
/// concat the two characters
/// parse number 
/// and aggegate its sum.

string[] unitsMap = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
string[] digitsMap = Enumerable.Range(0, 10).Select(x => x.ToString()).ToArray();
string pattern = string.Join("|", unitsMap.Concat(digitsMap));

var sum = File.ReadAllLines(@"../input.txt")
        .Select(line => new[]
        {
            Regex.Match(line, pattern).Value,
            Regex.Match(line, pattern, RegexOptions.RightToLeft).Value
        })
        .Select(line => line.Select(
            phrase => (char)('0' + (Array.IndexOf(unitsMap, phrase) == -1
                ? Array.IndexOf(digitsMap, phrase)
                : Array.IndexOf(unitsMap, phrase)))
        ))
        .Select(x => new string(x.ToArray()))
        .Select(int.Parse)
        .Aggregate((a,b) => a + b);

Console.WriteLine(sum);
