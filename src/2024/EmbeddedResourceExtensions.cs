using System.Text;

namespace Klinkby.AoC2024;

internal static class EmbeddedResourceExtensions
{
    public static IReadOnlyList<string> ReadAllLines(this EmbeddedResource embeddedResource)
    {
        using Stream stream = embeddedResource.GetStream();
        using StreamReader sr = new(stream, Encoding.UTF8, leaveOpen: true);
        List<string> lines = [];
        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine();
            if (line is null)
            {
                continue;
            }

            lines.Add(line);
        }

        return lines.AsReadOnly();
    }
}