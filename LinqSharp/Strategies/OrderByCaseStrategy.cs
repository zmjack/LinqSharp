using NStandard;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.Strategies
{
    public class OrderByCaseStrategy<TEntity, TRet> : IOrderStrategy<TEntity>
    {
        public Expression<Func<TEntity, int>> StrategyExpression { get; }

        public OrderByCaseStrategy(
            Expression<Func<TEntity, TRet>> memberExp,
            TRet[] orderValues)
        {
            var valueLenth = orderValues.Length;
            var lambdaExp = orderValues.Reverse().AsKvPairs().Aggregate(null as Expression, (acc, kv) =>
            {
                var compareExp = Expression.Equal(memberExp.Body, Expression.Constant(kv.Value));

                if (acc is null)
                {
                    return
                        Expression.Condition(
                            compareExp,
                            Expression.Constant(valueLenth - 1 - kv.Key),
                            Expression.Constant(valueLenth));
                }
                else
                {
                    return
                        Expression.Condition(
                            compareExp,
                            Expression.Constant(valueLenth - 1 - kv.Key),
                            acc);
                }
            });

            StrategyExpression =
                Expression.Lambda<Func<TEntity, int>>(lambdaExp, memberExp.Parameters);
        }

    }
}
