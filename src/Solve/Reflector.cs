using System.Reflection;

namespace Solve;

internal static class Reflector
{
    public static T CreateInstance<T>(string typeName) where T : class
    {
        Type solverType = Type.GetType(typeName) ??
                          throw new InvalidOperationException($"{typeName} not found");
        return Activator.CreateInstance(solverType) as T ??
               throw new InvalidOperationException($"{solverType} not instantiable");
    }

    public static async Task<IList<string>> ReadResourceTextLinesAsync(string assemblyName, string name, CancellationToken cancellationToken)
    {
        Assembly assembly = Assembly.Load(assemblyName);
        await using Stream inputStream =
            assembly.GetManifestResourceStream(name) ??
                 throw new InvalidOperationException(
                     $"{name} resource not found in {assembly.FullName}");

        using StreamReader sr = new(inputStream);
        List<string> lines = [];
        while (!sr.EndOfStream)
        {
            string? line = await sr.ReadLineAsync(cancellationToken);
            if (line is not null)
            {
                lines.Add(line);
            }
        }

        return lines;
    }
}