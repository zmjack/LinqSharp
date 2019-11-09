using System;
using System.Linq.Expressions;

namespace NLinq.Strategies
{
    public interface IWhereStrategy<TEntity>
    {
        Expression<Func<TEntity, bool>> StrategyExpression { get; }
    }

}
