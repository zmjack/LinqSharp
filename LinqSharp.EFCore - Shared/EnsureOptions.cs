using System;
using System.Linq.Expressions;

namespace LinqSharp.EFCore
{
    public class EnsureOptions<TEntity>
        where TEntity : class, new()
    {
        public Action<TEntity> SetEntity { get; set; }
        public Expression<Func<TEntity, bool>> Predicate { get; set; }
    }
}
