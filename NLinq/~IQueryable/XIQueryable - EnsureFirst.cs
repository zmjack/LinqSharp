using Microsoft.EntityFrameworkCore;
using NLinq.Strategies;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NLinq
{
    public static partial class XIQueryable
    {
        public static TEntity EnsureFirst<TEntity>(this IQueryable<TEntity> @this, DbContext context, EnsureCondition<TEntity>[] ensureConditions)
            where TEntity : new() => EnsureFirst(@this, context, ensureConditions, out _);
        public static TEntity EnsureFirst<TEntity>(this IQueryable<TEntity> @this, DbContext context, EnsureCondition<TEntity>[] ensureConditions, out bool isCreated)
            where TEntity : new()
        {
            var parameter = ensureConditions[0].Expression.Parameters[0];

            var predicate = ensureConditions.Select(x => Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(
                    x.Expression.RebindParameter(x.Expression.Parameters[0], parameter).Body.For(body => (body as UnaryExpression)?.Operand ?? body),
                    Expression.Constant(x.ExpectedValue)),
                parameter)).LambdaJoin(Expression.AndAlso);

            TEntity ret = @this.FirstOrDefault(predicate);
            if (ret == null)
            {
                var item = new TEntity();
                foreach (var pair in ensureConditions)
                {
                    var propName = (pair.Expression.Body.For(body => (body as UnaryExpression)?.Operand ?? body) as MemberExpression).Member.Name;
                    var prop = typeof(TEntity).GetProperty(propName);
                    prop.SetValue(item, pair.ExpectedValue);
                }
                context.Add(item);
                context.SaveChanges();

                ret = @this.FirstOrDefault(predicate);
                isCreated = true;
                return ret;
            }
            else
            {
                isCreated = false;
                return ret;
            }
        }

    }
}
