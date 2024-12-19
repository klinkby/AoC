using MathNet.Numerics.LinearAlgebra;

namespace Klinkby.AoC2024;

public partial class Day13
{
    [Test]
    public async Task Puzzle1()
    {
        var input = EmbeddedResource.input_day13_txt.ReadAllLines();
        Point cost = new(3, 1);
        int total = (
            from Machine machine in input
                .ParseInts(Splitter())
                .Select((x, i) => (x[0], x[1], i % 3, i / 3))
                .GroupBy(x => x.Item4)
                .Select(ParseMachine)
            let solveResult = SolvePushes(machine)
            where solveResult is not null
            select solveResult.Value
        ).Sum(pushes => pushes.X * cost.X + pushes.Y * cost.Y);
        await Assert.That(total).IsEqualTo(25629);
    }

    private static Point? SolvePushes(Machine machine)
    {
        var pushes = Matrix<double>.Build.DenseOfArray(new double[,]
        {
            { machine.A.X, machine.B.X },
            { machine.A.Y, machine.B.Y }
        }).Solve(Vector<double>.Build.DenseOfArray([machine.Prize.X, machine.Prize.Y]));
        Point iPushes = new((int)Math.Round(pushes[0]), (int)Math.Round(pushes[1]));
        bool accurate = machine.A.X * iPushes.X + machine.B.X * iPushes.Y == machine.Prize.X
                        && machine.A.Y * iPushes.X + machine.B.Y * iPushes.Y == machine.Prize.Y;
        return !accurate ? null : iPushes;
    }

    private static Machine ParseMachine(IGrouping<int, (int, int, int, int)> grp)
    {
        var lines = grp.ToArray();
        Point buttonA = new(lines[0].Item1, lines[0].Item2);
        Point buttonB = new(lines[1].Item1, lines[1].Item2);
        Point prize = new(lines[2].Item1, lines[2].Item2);
        return new Machine(buttonA, buttonB, prize);
    }

    [GeneratedRegex(@"(^[^:]+:\ X[+=])|(,\ Y[+=])",
        RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex Splitter();

    private readonly record struct Machine(Point A, Point B, Point Prize);
}