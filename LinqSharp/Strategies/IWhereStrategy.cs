using System;
using System.Linq.Expressions;

namespace LinqSharp.Strategies
{
    public interface IWhereStrategy<TEntity>
    {
        Expression<Func<TEntity, bool>> StrategyExpression { get; }
    }

}
