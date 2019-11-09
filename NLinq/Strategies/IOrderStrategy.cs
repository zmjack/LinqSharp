using System;
using System.Linq.Expressions;

namespace NLinq.Strategies
{
    public interface IOrderStrategy<TEntity>
    {
        Expression<Func<TEntity, int>> StrategyExpression { get; }
    }

}
