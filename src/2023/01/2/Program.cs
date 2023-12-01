using System.Text.RegularExpressions;

/// Read the input text file,
/// concat the first and last digit on each line
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
