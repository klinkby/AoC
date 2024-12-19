using MathNet.Numerics.LinearAlgebra;

namespace Klinkby.AoC2024;

public partial class Day13
{
    private readonly static Point Cost = new(3, 1);

    [Test]
    public async Task Puzzle1()
    {
        var input = EmbeddedResource.input_day13_txt.ReadAllLines();
        long total = GetPriceOfPushes(input);
        await Assert.That(total).IsEqualTo(25629);
    }

    [Test]
    public async Task Puzzle2()
    {
        var input = EmbeddedResource.input_day13_txt.ReadAllLines();
        long total = GetPriceOfPushes(input, basePrice: 10_000_000_000_000L);
        await Assert.That(total).IsEqualTo(107487112929999L);
    }

    private static long GetPriceOfPushes(IEnumerable<string> input, long basePrice = 0L)
    {
        long total = (
            from Machine machine in ParseMachines(input, basePrice)
            let solveResult = SolvePushes(machine)
            where solveResult is not null
            select solveResult.Value
        ).Sum(static pushes => pushes.A * Cost.X + pushes.B * Cost.Y);
        return total;
    }

    private static (long A, long B)? SolvePushes(Machine machine)
    {
        var pushes = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                { machine.A.X, machine.B.X },
                { machine.A.Y, machine.B.Y }
            }).Solve(
                Vector<double>.Build.DenseOfArray(
                [
                    machine.PrizeX, 
                    machine.PrizeY
                ]));
        (long A, long B) iPushes = ((long)Math.Round(pushes[0]), (long)Math.Round(pushes[1]));
        bool accurate = machine.A.X * iPushes.A + machine.B.X * iPushes.B == machine.PrizeX
                        && machine.A.Y * iPushes.A + machine.B.Y * iPushes.B == machine.PrizeY;
        return !accurate ? null : iPushes;
    }

    private static IEnumerable<Machine> ParseMachines(IEnumerable<string> input, long basePrice)
    {
        return input.ParseInts(Splitter())
            .Select(static (x, i) => (x[0], x[1], i % 3, i / 3))
            .GroupBy(static x => x.Item4)
            .Select(x => ParseMachine(x, basePrice));
    }

    private static Machine ParseMachine(IGrouping<int, (int, int, int, int)> grp, long basePrice)
    {
        var lines = grp.ToArray();
        Point buttonA = new(lines[0].Item1, lines[0].Item2);
        Point buttonB = new(lines[1].Item1, lines[1].Item2);
        Point prize = new(lines[2].Item1, lines[2].Item2);
        return new Machine(buttonA, buttonB, prize.X + basePrice, prize.Y + basePrice);
    }

    [GeneratedRegex(@"(^[^:]+:\ X[+=])|(,\ Y[+=])",
        RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex Splitter();

    private readonly record struct Machine(Point A, Point B, long PrizeX, long PrizeY);
}