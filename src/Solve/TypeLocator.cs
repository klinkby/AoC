namespace Solve;

internal sealed record TypeLocator(PuzzleId Id)
{
    public string AssemblyName => $"Solve.AoC{Id.Year}";

    public string InputResourceName => $"{AssemblyName}.input.{Id.Day:00}.txt";

    public string PuzzleTypeName
    {
        get
        {
            string assemblyName = AssemblyName;
            return $"{assemblyName}.Day{Id.Day:00}Puzzle{Id.Puzzle}, {assemblyName}";
        }
    }
}