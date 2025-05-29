namespace LinqSharp.Design;

public struct LocalSortRule<TSource>(Func<TSource, object> selector, bool descending = false)
{
    public Func<TSource, object> Selector { get; set; } = selector;
    public bool Descending { get; set; } = descending;
}
