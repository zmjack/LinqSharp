namespace LinqSharp.Comparers;

public class SequenceComparer<T> : IEqualityComparer<IEnumerable<T>>
{
    public SequenceComparer()
    {
    }

    public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
    {
        if (x is null) return y is null;
        if (y is null) return false;
        return x.SequenceEqual(y);
    }

    public int GetHashCode(IEnumerable<T> obj)
    {
        return obj.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode()));
    }
}
