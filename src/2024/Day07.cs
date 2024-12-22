namespace Klinkby.AoC2024;

public partial class Day07
{
    [Test]
    public async Task Puzzle1()
    {
        IEnumerable<List<long>> equations = GetEquations();
        long sum = Brrr(equations, [Add, Mul]);

        await Assert.That(sum).IsEqualTo(5512534574980);
    }

    [Test]
    public async Task Puzzle2()
    {
        IEnumerable<List<long>> equations = GetEquations();
        long sum = Brrr(equations, [Add, Mul, Concat]);

        await Assert.That(sum).IsEqualTo(328790210468594);
    }

    private static long Brrr(IEnumerable<List<long>> equations, Func<long, long, long>[] operators)
    {
        long sum = 0;
        int opCount = operators.Length;
        foreach (List<long> equation in equations)
        {
            long expected = equation[0];
            int operands = equation.Count - 1;
            foreach (int[] permutations in GetPermutations(operands, opCount))
            {
                long actual = equation[1];
                for (int j = 0; j < permutations.Length; j++)
                {
                    int permutation = permutations[j];
                    actual = operators[permutation](actual, equation[j + 2]);
                }

                if (actual == expected)
                {
                    sum += actual;
                    break;
                }
            }
        }

        return sum;
    }
    
    private static IEnumerable<int[]> GetPermutations(int operands, int opCount)
    {
        int[] ops = new int[operands - 1];
        while (true)
        {
            yield return ops;
            for (int j = 0;; j++)
            {
                ops[j]++;
                if (ops[j] != opCount)
                {
                    break;
                }

                ops[j] = 0;
                if (j == ops.Length - 1)
                {
                    yield break;
                }
            }
        }
    }

    private static long Add(long a, long b) => a + b;
    private static long Mul(long a, long b) => a * b;
    private static long Concat(long a, long b) => long.Parse(
        a.ToString(CultureInfo.InvariantCulture) +
        b.ToString(CultureInfo.InvariantCulture),
        CultureInfo.InvariantCulture);

    private static IEnumerable<List<long>> GetEquations() =>
        EmbeddedResource.day07_txt.ReadAllLines()
            .ParseLongs(Splitter());

    [GeneratedRegex(@":\s|\s",
        RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex Splitter();
}