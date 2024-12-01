using System.Text;

namespace Klinkby.AoC2024.Extensions;

internal static class EmbeddedResourceExtensions
{
    public static ReadOnlySpan<string> ReadAllLines(this EmbeddedResource embeddedResource)
    {
        using Stream stream = embeddedResource.GetStream();
        using StreamReader sr = new(stream, Encoding.UTF8, leaveOpen: true);
        List<string> lines = new(1000);
        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine();
            if (line is null)
            {
                continue;
            }

            lines.Add(line);
        }

        return lines.ToArray();
    }
}