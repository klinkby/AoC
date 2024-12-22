namespace Klinkby.AoC2024;

public class Day17
{
    private readonly static string[] Input = EmbeddedResource.day17_txt.ReadAllLines().ToArray();

    [Test]
    public async Task Puzzle1()
    {
        Cpu cpu = ParseCpu();
        int[] program = ParseProgram();
        int[] expected = [1, 0, 2, 0, 5, 7, 2, 1, 3];
        bool same = Run(cpu, program).SequenceEqual(expected);
        await Assert.That(same).IsTrue();
    }

    private static IEnumerable<int> Run(Cpu cpu, int[] program)
    {
        while (cpu.PC < program.Length)
        {
            Opcode opcode = (Opcode)program[cpu.PC];
            int operand = program[cpu.PC + 1];
            var result = cpu.Execute(opcode, operand);
            if (result.StdOut.HasValue)
            {
                yield return result.StdOut.Value;
            }

            cpu = result.Cpu;
        }
    }

    private static Cpu ParseCpu() =>
        new(
            Input.Take(1).ParseInts().First()[0],
            Input.Skip(1).Take(1).ParseInts().First()[0],
            Input.Skip(2).Take(1).ParseInts().First()[0]
        );

    private static int[] ParseProgram() =>
        [.. Input.TakeLast(1).ParseInts().First()];


    private readonly record struct Cpu(int A, int B, int C, int PC = 0)
    {
        private const int Step = 2;

        public (Cpu Cpu, int? StdOut) Execute(Opcode opcode, int operand)
        {
            Func<int, (Cpu, int?)> execute = opcode switch
            {
                Opcode.Adv => Adv,
                Opcode.Bxl => Bxl,
                Opcode.Bst => Bst,
                Opcode.Jnz => Jnz,
                Opcode.Bcx => Bcx,
                Opcode.Out => Out,
                Opcode.Bdv => Bdv,
                Opcode.Cdv => Cdv,
                _ => throw new ArgumentOutOfRangeException(nameof(opcode))
            };
            return execute(operand);
        }

        private (Cpu, int?) Adv(int operand) =>
            (this with
            {
                A = A / (1 << GetCombo(operand)),
                PC = PC + Step
            }, null);

        private (Cpu, int?) Bxl(int operand) =>
            (this with
            {
                B = B ^ operand,
                PC = PC + Step
            }, null);

        private (Cpu, int?) Bst(int operand) =>
            (this with
            {
                B = GetCombo(operand) % 8,
                PC = PC + Step
            }, null);

        private (Cpu, int?) Jnz(int operand) =>
            (this with
            {
                PC = A == 0 ? PC + Step : operand
            }, null);

        private (Cpu, int?) Bcx(int _) =>
            (this with
            {
                B = B ^ C,
                PC = PC + Step
            }, null);

        private (Cpu, int?) Out(int operand) => (this with { PC = PC + Step }, GetCombo(operand) % 8);

        private (Cpu, int?) Bdv(int operand) =>
            (this with
            {
                B = A / (1 << GetCombo(operand)),
                PC = PC + Step
            }, null);

        private (Cpu, int?) Cdv(int operand) =>
            (this with
            {
                C = A / (1 << GetCombo(operand)),
                PC = PC + Step
            }, null);

        private int GetCombo(int operand)
        {
            return operand switch
            {
                >= 0 and <= 3 => operand,
                4 => A,
                5 => B,
                6 => C,
                _ => throw new ArgumentOutOfRangeException(nameof(operand))
            };
        }
    }

    private enum Opcode
    {
        Adv = 0,
        Bxl,
        Bst,
        Jnz,
        Bcx,
        Out,
        Bdv,
        Cdv
    }
}