using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.EFCore
{
    public class EnsureCondition<TEntity> where TEntity : new()
    {
        public List<EnsureConditionUnit<TEntity>> UnitList = new List<EnsureConditionUnit<TEntity>>();

        public object this[string propName]
        {
            set
            {
                var parameter = Expression.Parameter(typeof(TEntity));
                var body = Expression.Property(parameter, propName);

                var expression = Expression.Lambda<Func<TEntity, object>>(body, parameter);
                UnitList.Add(new EnsureConditionUnit<TEntity>(expression, value));
            }
        }

        public object this[Expression<Func<TEntity, object>> expression]
        {
            set => UnitList.Add(new EnsureConditionUnit<TEntity>(expression, value));
        }

        public Expression<Func<TEntity, bool>> GetExpression()
        {
            if (!UnitList.Any()) throw new InvalidOperationException("Missing conditional unit.");

            var units = UnitList.ToArray();
            var parameter = units[0].UnitExpression.Parameters[0];
            var predicate = units.Select(x => Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(
                    x.UnitExpression.RebindParameter(x.UnitExpression.Parameters[0], parameter).Body.For(body => (body as UnaryExpression)?.Operand ?? body),
                    Expression.Constant(x.ExpectedValue)),
                parameter)).LambdaJoin(Expression.AndAlso);
            return predicate;
        }

    }

}
