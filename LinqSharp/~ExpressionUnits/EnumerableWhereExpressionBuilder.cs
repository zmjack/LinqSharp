using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public class EnumerableWhereExpressionBuilder<TSource> : WhereExpressionBuilder<TSource>
    {
        public IEnumerable<TSource> Enumerable { get; set; }

        public EnumerableWhereExpressionBuilder(IEnumerable<TSource> enumerable)
        {
            Enumerable = enumerable;
        }

        public EnumerableWhereExpressionBuilder(IEnumerable<TSource> enumerable, Expression<Func<TSource, bool>> predicate)
        {
            Enumerable = enumerable;
            Parameter = predicate.Parameters[0];
            Expression = predicate;
        }

        public IEnumerable<TSource> End() => Enumerable.Where(Expression.Compile());

    }
}
