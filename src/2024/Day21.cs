using System.Diagnostics;

namespace Klinkby.AoC2024;

public class Day21
{
    private readonly static string[] Input = EmbeddedResource.day21_txt.ReadAllLines().ToArray();

    [Test]
    public async Task Puzzle1()
    {
        await Assert.That(GetSequence("029A").Length)
            .IsEqualTo("<vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A".Length);
        await Assert.That(GetSequence("980A").Length)
            .IsEqualTo("<v<A>>^AAAvA^A<vA<AA>>^AvAA<^A>A<v<A>A>^AAAvA<^A>A<vA>^A<A>A".Length);
        await Assert.That(GetSequence("179A").Length)
            .IsEqualTo("<v<A>>^A<vA<A>>^AAvAA<^A>A<v<A>>^AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A".Length);
        await Assert.That(GetSequence("456A").Length)
            .IsEqualTo("<v<A>>^AA<vA<A>>^AAvAA<^A>A<vA>^A<A>A<vA>^A<A>A<v<A>A>^AAvA<^A>A".Length);
        await Assert.That(GetSequence("379A").Length)
            .IsEqualTo("<v<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A".Length);

        int sum = (from code in Input
            let symbols = code.ToCharArray()
            let numeric = int.Parse(symbols.Where(ch => ch is >= '0' and <= '9')
                .ToArray(), CultureInfo.InvariantCulture)
            let sequence = GetSequence(code)
            select numeric * sequence.Length).Sum();

        await Assert.That(sum).IsGreaterThan(177288);
        await Assert.That(sum).IsLessThan(231482);
        await Assert.That(sum).IsEqualTo(126384);
    }

    private static string GetSequence(string code)
    {
        var moves =
            new DirectionalRobot().GetMoves(
                new DirectionalRobot().GetMoves(
                    new NumericalRobot().GetMoves(code)));
        string sequence = new(moves.ToArray());
        return sequence;
    }

    private interface IRobot
    {
        IEnumerable<Size> GetMoves(char symbol);
        IRobot Move(Size move);
    }

    private readonly record struct NumericalRobot(Point Position) : IRobot
    {
        private readonly static Keypad Keypad = new(new Dictionary<char, Point>
            {
                ['7'] = new(0, 0), ['8'] = new(1, 0), ['9'] = new(2, 0),
                ['4'] = new(0, 1), ['5'] = new(1, 1), ['6'] = new(2, 1),
                ['1'] = new(0, 2), ['2'] = new(1, 2), ['3'] = new(2, 2),
                /* (0, 3) miss */  ['0'] = new(1, 3), ['A'] = new(2, 3)
            });

        public NumericalRobot() : this(new Point(2, 3))
        {
        }

        public IRobot Move(Size move) => new NumericalRobot(Position + move);
        
        public IEnumerable<Size> GetMoves(char symbol) => Keypad.GetMoves(Position, symbol, Position.Y == 3);

        public IEnumerable<char> GetMoves(IEnumerable<char> symbols) => Keypad.GetMoves(this, symbols);
    }

    private readonly record struct DirectionalRobot(Point Position) : IRobot
    {
        private readonly static Keypad Keypad = new(new Dictionary<char, Point>
            {
                /* (0, 0) miss */  ['^'] = new(1, 0), ['A'] = new(2, 0),
                ['<'] = new(0, 1), ['v'] = new(1, 1), ['>'] = new(2, 1)
            });

        public DirectionalRobot() : this(new Point(2, 0))
        {
        }

        public IRobot Move(Size move) => new DirectionalRobot(Position + move);

        public IEnumerable<Size> GetMoves(char symbol) => Keypad.GetMoves(Position, symbol, Position.Y == 0);
        public IEnumerable<char> GetMoves(IEnumerable<char> symbols) => Keypad.GetMoves(this, symbols);
    }

    private readonly record struct Keypad(IReadOnlyDictionary<char, Point> Buttons)
    {
        private readonly static Dictionary<Size, char> MoveSymbols = new()
        {
            [new Size(0, -1)] = '^',
            [new Size(0,  1)] = 'v',
            [new Size(-1, 0)] = '<',
            [new Size( 1, 0)] = '>'
        };
        private readonly Dictionary<Point, char> ButtonsReverse = Buttons.ToDictionary(x => x.Value, x => x.Key);

        public static IEnumerable<char> GetMoves(IRobot robot, IEnumerable<char> input)
        {
            foreach (char c in input)
            {
                foreach (Size move in robot.GetMoves(c))
                {
                    yield return MoveSymbols[move];
                    robot = robot.Move(move);
                }

                yield return 'A';
            }
        }

        public IEnumerable<Size> GetMoves(Point p, char symbol, bool isLimitedRow)
        {
            Point target = Buttons[symbol];
            if (target == p)
            {
                yield break;
            }

            Size moveH = new(p.X < target.X ? 1 : p.X > target.X ? -1 : 0, 0);
            Size moveV = new(0, p.Y < target.Y ? 1 : p.Y > target.Y ? -1 : 0);
            if (isLimitedRow)
            {
                while (p.Y != target.Y && CanMove(p, moveV))
                {
                    yield return moveV;
                    p += moveV;
                }
            }

            while (p != target)
            {
                while (p.X != target.X && CanMove(p, moveH))
                {
                    yield return moveH;
                    p += moveH;
                }

                while (p.Y != target.Y && CanMove(p, moveV))
                {
                    yield return moveV;
                    p += moveV;
                }
            }
        }

        private bool CanMove(Point from, Size move)
        {
            bool moveValid = Math.Abs(move.Width) <= 1 &&
                             Math.Abs(move.Height) <= 1 &&
                             move is not { Width: 0, Height: 0 };
            return moveValid && ButtonsReverse.ContainsKey(from + move);
        }
    }
}