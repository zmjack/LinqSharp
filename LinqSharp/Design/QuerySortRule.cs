using System.Linq.Expressions;

namespace LinqSharp.Design;

public struct QuerySortRule<TSource>(Expression<Func<TSource, object>> selector, bool descending = false)
{
    public Expression<Func<TSource, object>> Selector { get; set; } = selector;
    public bool Descending { get; set; } = descending;
}
