using System;
using System.Linq;
using System.Linq.Expressions;

namespace NLinq
{
    public class QueryableWhereExpressionBuilder<TSource> : WhereExpressionBuilder<TSource>
    {
        public IQueryable<TSource> Queryable { get; set; }

        public QueryableWhereExpressionBuilder(IQueryable<TSource> queryable)
        {
            Queryable = queryable;
        }

        public QueryableWhereExpressionBuilder(IQueryable<TSource> queryable, Expression<Func<TSource, bool>> predicate)
        {
            Queryable = queryable;
            Parameter = predicate.Parameters[0];
            Expression = predicate;
        }

        public IQueryable<TSource> End() => Queryable.Where(Expression);

    }
}
