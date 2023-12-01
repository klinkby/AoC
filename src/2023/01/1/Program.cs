using System.Text.RegularExpressions;

/// Read the input text file,
/// concat the first and last digit on each line
/// parse number 
/// and aggegate its sum.

var sum = File.ReadAllLines(@"../input.txt")
              .Select(line => string.Empty + Regex.Match(line, @"\d").Value + Regex.Match(line, @"\d", RegexOptions.RightToLeft).Value)
              .Select(int.Parse)
              .Aggregate((a,b) => a + b);

Console.WriteLine(sum);
