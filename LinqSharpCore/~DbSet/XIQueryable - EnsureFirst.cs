using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        public static TEntity EnsureFirst<TEntity>(this DbSet<TEntity> @this, EnsureCondition<TEntity> condition, Action<TEntity> setEntity = null)
            where TEntity : class, new()
            => EnsureMany(@this, new[] { condition }, setEntity).First();

    }
}
