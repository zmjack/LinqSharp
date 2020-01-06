using LinqSharp.Strategies;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        #region Return DateTime
        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            Expression<Func<TEntity, DateTime>> endExp)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, startExp, endExp));

        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            DateTime start,
            Expression<Func<TEntity, DateTime>> endExp)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, start, endExp));

        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            DateTime end)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, startExp, end));

        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            DateTime start,
            DateTime end)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, start, end));
        #endregion

        #region Return DateTime?
        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            Expression<Func<TEntity, DateTime>> endExp)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, startExp, endExp));

        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            DateTime start,
            Expression<Func<TEntity, DateTime>> endExp)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, start, endExp));

        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            DateTime end)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, startExp, end));

        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            DateTime start,
            DateTime end)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, start, end));
        #endregion

    }
}
