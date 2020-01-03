using NStandard;
using System;
using System.Linq.Expressions;

namespace NLinq
{
    public class WhereExpressionBuilder<TSource>
    {
        public ParameterExpression Parameter { get; set; }
        public Expression<Func<TSource, bool>> Expression { get; set; }

        public WhereExpressionBuilder()
        {
        }

        public WhereExpressionBuilder(Expression<Func<TSource, bool>> predicate)
        {
            Parameter = predicate.Parameters[0];
            Expression = predicate;
        }

        public WhereExpressionBuilder<TSource> Set(Expression<Func<TSource, bool>> predicate)
        {
            if (Expression == null)
            {
                Parameter = predicate.Parameters[0];
                Expression = predicate;
            }
            else throw new InvalidOperationException("This method can only be used as initialization, use `And` or `Or` instead.");
            return this;
        }
        public WhereExpressionBuilder<TSource> SetDynamic(Action<DynamicExpressionBuilder<TSource>> build)
        {
            var builer = new DynamicExpressionBuilder<TSource>().Then(x => build(x));
            return Set(builer.Lambda);
        }

        public WhereExpressionBuilder<TSource> And(Expression<Func<TSource, bool>> predicate)
        {
            if (Expression == null)
            {
                Parameter = predicate.Parameters[0];
                Expression = predicate;
            }
            else
            {
                Expression = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(
                    System.Linq.Expressions.Expression.AndAlso(Expression.Body, predicate.Body.RebindParameter(predicate.Parameters[0], Parameter)), Parameter);
            }
            return this;
        }
        public WhereExpressionBuilder<TSource> AndDynamic(Action<DynamicExpressionBuilder<TSource>> build)
        {
            var builer = new DynamicExpressionBuilder<TSource>().Then(x => build(x));
            return And(builer.Lambda);
        }

        public WhereExpressionBuilder<TSource> Or(Expression<Func<TSource, bool>> predicate)
        {
            if (Expression == null)
            {
                Parameter = predicate.Parameters[0];
                Expression = predicate;
            }
            else
            {
                Expression = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(
                    System.Linq.Expressions.Expression.OrElse(Expression.Body, predicate.Body.RebindParameter(predicate.Parameters[0], Parameter)), Parameter);
            }
            return this;
        }
        public WhereExpressionBuilder<TSource> OrDynamic(Action<DynamicExpressionBuilder<TSource>> build)
        {
            var builer = new DynamicExpressionBuilder<TSource>().Then(x => build(x));
            return Or(builer.Lambda);
        }

    }
}
