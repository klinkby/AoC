global using System.Reflection;

namespace Solve;

internal static class Reflector
{
    public static IPuzzleSolver CreatePuzzleSolver(PuzzleId id)
    {
        Type solverType = GetPuzzleSolverType(id);
        return Activator.CreateInstance(solverType) as IPuzzleSolver ??
               throw new InvalidOperationException($"{solverType} not instantiable");
    }

    public static async Task<IList<string>> GetInputAsync(PuzzleId id, CancellationToken cancellationToken)
    {
        string assemblyName = GetAssemblyName(id.Year);
        string resourceName = $"{id.Year}.input.{id.Day:00}.txt";
        await using Stream inputStream = Assembly
                                             .Load(assemblyName)
                                             .GetManifestResourceStream(resourceName) ??
                                         throw new InvalidOperationException(
                                             $"{resourceName} resource not found in {assemblyName}");
        using var sr = new StreamReader(inputStream);
        List<string> lines = [];
        while (!sr.EndOfStream)
        {
            string? line = await sr.ReadLineAsync(cancellationToken);
            if (line is not null) lines.Add(line);
        }

        return lines;

    }

    private static string GetAssemblyName(int year) => $"aoc{year}";

    private static string GetPuzzleTypeName(PuzzleId id)
    {
        string assemblyName = GetAssemblyName(id.Year);
        return $"{assemblyName}.Day{id.Day:00}Puzzle{id.Puzzle}, {assemblyName}";
    }

    private static Type GetPuzzleSolverType(PuzzleId id)
    {
        string typeName = GetPuzzleTypeName(id);
        return Type.GetType(typeName) ??
               throw new InvalidOperationException($"Solver {typeName} not found");
    }
}