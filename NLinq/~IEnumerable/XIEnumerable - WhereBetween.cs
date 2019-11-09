using NLinq.Strategies;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NLinq
{
    public static partial class XIEnumerable
    {
        #region Return DateTime
        public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            Expression<Func<TEntity, DateTime>> endExp)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, startExp, endExp));

        public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            DateTime start,
            Expression<Func<TEntity, DateTime>> endExp)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, start, endExp));

        public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            DateTime end)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, startExp, end));

        public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            DateTime start,
            DateTime end)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, start, end));
        #endregion

        #region Return DateTime?
        public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            Expression<Func<TEntity, DateTime>> endExp)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, startExp, endExp));

        public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            DateTime start,
            Expression<Func<TEntity, DateTime>> endExp)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, start, endExp));

        public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            DateTime end)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, startExp, end));

        public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            DateTime start,
            DateTime end)
            => @this.WhereStrategy(new WhereBetweenStrategy<TEntity>(memberExp, start, end));
        #endregion

    }
}
