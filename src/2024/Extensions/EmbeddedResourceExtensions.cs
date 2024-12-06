using System.Text;

namespace Klinkby.AoC2024.Extensions;

internal static class EmbeddedResourceExtensions
{
    public static ReadOnlySpan<char> ReadToEnd(this EmbeddedResource embeddedResource)
    {
        using Stream stream = embeddedResource.GetStream();
        using StreamReader sr = new(stream, Encoding.UTF8, leaveOpen: true);
        return sr.ReadToEnd();
    }

    public static IEnumerable<string> ReadAllLines(this EmbeddedResource embeddedResource)
    {
        using Stream stream = embeddedResource.GetStream();
        using StreamReader sr = new(stream, Encoding.UTF8, leaveOpen: true);
        while (sr.ReadLine() is { } text)
        {
            yield return text;
        }
    }
}