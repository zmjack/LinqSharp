using System;
using System.Linq;
using System.Linq.Expressions;

namespace NLinq
{
    public static partial class XIQueryable
    {
        public static IQueryable<TSource> WhereMin<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            if (source.Any())
            {
                var min = source.Min(selector);
                var whereExp = Expression.Lambda<Func<TSource, bool>>(
                    Expression.Equal(selector.Body, Expression.Constant(min, typeof(TResult))), selector.Parameters);
                return source.Where(whereExp);
            }
            else return source;
        }

    }
}
