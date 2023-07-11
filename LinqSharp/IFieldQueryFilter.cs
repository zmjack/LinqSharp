using System;
using System.Linq.Expressions;

namespace LinqSharp
{
    public interface IFieldQueryFilter<T>
    {
        Expression<Func<T, bool>> Predicate { get; }
    }
}
