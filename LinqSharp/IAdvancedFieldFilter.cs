using LinqSharp.Query;

namespace LinqSharp;

//TODO: Delete
[Obsolete("Use IAdvancedFieldFilter instead.", true)]
public interface IExtraFieldFilter<T>
{
    IEnumerable<QueryExpression<T>> Filter(QueryHelper<T> h);
}

public interface IAdvancedFieldFilter<T>
{
    IEnumerable<QueryExpression<T>> Filter(QueryHelper<T> h);
}
