using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public class QueryableWhereExpressionBuilder<TSource> : WhereExpressionBuilder<TSource>
    {
        public IQueryable<TSource> Queryable { get; set; }

        public QueryableWhereExpressionBuilder(IQueryable<TSource> queryable)
        {
            Queryable = queryable;
        }

        public IQueryable<TSource> Build() => Queryable.Where(Expression);

    }
}
