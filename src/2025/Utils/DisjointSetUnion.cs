namespace Klinkby.AoC2025.Utils;

/// <summary>
///     Represents a Disjoint Set Union (Union-Find) data structure,
///     for performing operations for merging sets and finding representatives of elements in the same set.
/// </summary>
internal readonly ref struct DisjointSetUnion
{
    private readonly Span<int> _parent;
    private readonly Span<int> _count;

    public DisjointSetUnion(Span<int> parent, Span<int> count)
    {
        _parent = parent;
        _count = count;
        for (var i = 0; i < parent.Length; i++)
        {
            _parent[i] = i;
            _count[i] = 1;
        }
    }

    public int Find(int x)
    {
        while (_parent[x] != x)
        {
            _parent[x] = _parent[_parent[x]]; // path halving
            x = _parent[x];
        }

        return x;
    }

    public bool Union(int a, int b)
    {
        a = Find(a);
        b = Find(b);
        if (a == b) return false;

        if (_count[a] < _count[b]) (a, b) = (b, a);
        _parent[b] = a;
        _count[a] += _count[b];
        return true;
    }

    public int Count(int x) => _count[Find(x)];
}