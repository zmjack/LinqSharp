using LinqSharp.Query;
using System.Collections.Generic;

namespace LinqSharp;

public interface IExtraFieldFilter<T>
{
    IEnumerable<QueryExpression<T>> Filter(QueryHelper<T> h);
}
