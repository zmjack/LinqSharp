using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        public static TEntity[] EnsureMany<TEntity>(this DbSet<TEntity> @this, EnsureCondition<TEntity>[] conditions, Action<TEntity> setEntity = null)
            where TEntity : class, new()
        {
            var context = (@this as DbSet<TEntity>).GetDbContext();
            var expressions = conditions.Select(x => x.GetExpression()).ToArray();

            var parameter = expressions[0].Parameters[0];
            var predicate = expressions.Each(exp => exp.RebindParameter(exp.Parameters[0], parameter)).LambdaJoin(Expression.OrElse);

            var exsists = @this.Where(predicate).ToArray();
            var conditionsForAdd = conditions.Where(condition => !exsists.Any(condition.GetExpression().Compile()));

            foreach (var conditionForAdd in conditionsForAdd)
            {
                var item = new TEntity();
                foreach (var unit in conditionForAdd.UnitList)
                {
                    var prop = typeof(TEntity).GetProperty(unit.PropName);
                    prop.SetValue(item, unit.ExpectedValue);
                }
                setEntity?.Invoke(item);
                context.Add(item);
            }
            context.SaveChanges();

            return @this.Where(predicate).ToArray();
        }

    }
}
