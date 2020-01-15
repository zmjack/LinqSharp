using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class XExpression
    {
        public static TExpression RebindParameter<TExpression>(this TExpression @this, ParameterExpression origin, ParameterExpression target)
            where TExpression : Expression
        {
            if (origin != target)
                return new ExpressionRebindVisitor(origin, target).Visit(@this) as TExpression;
            else return @this;
        }

        public static TLambdaExpression LambdaJoin<TLambdaExpression>(this IEnumerable<TLambdaExpression> @this, Func<Expression, Expression, BinaryExpression> binary)
            where TLambdaExpression : LambdaExpression
        {
            var parameter = @this.First().Parameters[0];

            return Expression.Lambda(@this.Aggregate(null as Expression, (acc, exp) =>
            {
                if (acc is null)
                    return exp.Body;
                else
                {
                    var rebindExp = RebindParameter(exp, exp.Parameters[0], parameter);
                    return binary(acc, rebindExp.Body);
                }
            }), parameter) as TLambdaExpression;
        }
    }
}
