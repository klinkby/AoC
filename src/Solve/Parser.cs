namespace Solve;

internal static class Parser
{
    public static PuzzleId ParseArguments(string[] args)
    {
        (int Min, int Max)[] ranges = [(2000, 3000), (1, 25), (1, 2)];

        if (args.Length != ranges.Length)
        {
            throw new ArgumentException("Expected 3 arguments");
        }

        int[] intArgs = Enumerable
            .Range(0, ranges.Length)
            .Select(i =>
                int.TryParse(args[i], out int result) &&
                    ranges[i].Min <= result &&
                    result <= ranges[i].Max
                    ? result
                    : throw new ArgumentOutOfRangeException(
                        args[i],
                        $"Out of range [{ranges[i].Min}..{ranges[i].Max}]"))
            .ToArray();

        return new PuzzleId(intArgs[0], intArgs[1], intArgs[2]);
    }
}