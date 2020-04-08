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

        public IEnumerable<TSource> Build() => Enumerable.Where(Expression.Compile());

    }
}
