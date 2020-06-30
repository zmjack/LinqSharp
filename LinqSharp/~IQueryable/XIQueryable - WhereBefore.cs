using LinqSharp.Strategies;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        public static IQueryable<TEntity> WhereBefore<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            Expression<Func<TEntity, DateTime>> beforeExp,
            bool includePoint = true)
        {
            return @this.Where(new WhereBeforeStrategy<TEntity>(memberExp, beforeExp, includePoint).StrategyExpression);
        }

        public static IQueryable<TEntity> WhereBefore<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            DateTime before,
            bool includePoint = true)
        {
            return @this.Where(new WhereBeforeStrategy<TEntity>(memberExp, before, includePoint).StrategyExpression);
        }

        public static IQueryable<TEntity> WhereBefore<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            Expression<Func<TEntity, DateTime>> beforeExp,
            bool liftNullToTrue, bool includePoint = true)
        {
            return @this.Where(new WhereBeforeStrategy<TEntity>(memberExp, beforeExp, liftNullToTrue, includePoint).StrategyExpression);
        }

        public static IQueryable<TEntity> WhereBefore<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            DateTime before,
            bool liftNullToTrue, bool includePoint = true)
        {
            return @this.Where(new WhereBeforeStrategy<TEntity>(memberExp, before, liftNullToTrue, includePoint).StrategyExpression);
        }

        public static IQueryable<TEntity> WhereBefore<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, object>> yearExp,
            Expression<Func<TEntity, object>> monthExp,
            Expression<Func<TEntity, object>> dayExp,
            DateTime before,
            bool includePoint = true)
        {
            return @this.Where(new WhereBeforeStrategy<TEntity>(yearExp, monthExp, dayExp, before, includePoint).StrategyExpression);
        }
    }
}
