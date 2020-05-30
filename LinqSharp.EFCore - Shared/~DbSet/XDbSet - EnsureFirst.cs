using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        public static TEntity EnsureFirst<TEntity>(this DbSet<TEntity> @this, EnsureCondition<TEntity> condition)
            where TEntity : class, new()
        {
            return EnsureMany(@this, new[] { condition }, null).First();
        }

        public static TEntity EnsureFirst<TEntity>(this DbSet<TEntity> @this, EnsureCondition<TEntity> condition, Action<EnsureOptions<TEntity>> initOptions)
            where TEntity : class, new()
        {
            return EnsureMany(@this, new[] { condition }, initOptions).First();
        }

    }
}
