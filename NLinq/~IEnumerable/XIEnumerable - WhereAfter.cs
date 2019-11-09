using NLinq.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NLinq
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TEntity> WhereAfter<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            Expression<Func<TEntity, DateTime>> afterExp,
            bool includePoint = true)
            => @this.WhereStrategy(new WhereAfterStrategy<TEntity>(memberExp, afterExp, includePoint));

        public static IEnumerable<TEntity> WhereAfter<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            DateTime after,
            bool includePoint = true)
            => @this.WhereStrategy(new WhereAfterStrategy<TEntity>(memberExp, after, includePoint));

        public static IEnumerable<TEntity> WhereAfter<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            Expression<Func<TEntity, DateTime>> afterExp,
            bool liftNullToTrue, bool includePoint = true)
            => @this.WhereStrategy(new WhereAfterStrategy<TEntity>(memberExp, afterExp, liftNullToTrue, includePoint));

        public static IEnumerable<TEntity> WhereAfter<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            DateTime after,
            bool liftNullToTrue, bool includePoint = true)
            => @this.WhereStrategy(new WhereAfterStrategy<TEntity>(memberExp, after, liftNullToTrue, includePoint));

        public static IEnumerable<TEntity> WhereAfter<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, object>> yearExp,
            Expression<Func<TEntity, object>> monthExp,
            Expression<Func<TEntity, object>> dayExp,
            DateTime after,
            bool includePoint = true)
        {
            string GetPart(TEntity x, Expression<Func<TEntity, object>> exp, int totalWidth)
            {
                return exp.Compile()(x).ToString().PadLeft(totalWidth, '0');
            }

            return @this.Where(x =>
            {
                if (includePoint)
                    return string.CompareOrdinal($"{GetPart(x, yearExp, 4)}-{GetPart(x, monthExp, 2)}-{GetPart(x, dayExp, 2)}", after.ToString("yyyy-MM-dd")) >= 0;
                else return string.CompareOrdinal($"{GetPart(x, yearExp, 4)}-{GetPart(x, monthExp, 2)}-{GetPart(x, dayExp, 2)}", after.ToString("yyyy-MM-dd")) > 0;
            });
        }

    }
}
