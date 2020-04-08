using System;
using System.Linq.Expressions;

namespace LinqSharp.Strategies
{
    public interface IOrderStrategy<TEntity>
    {
        Expression<Func<TEntity, int>> StrategyExpression { get; }
    }

}
