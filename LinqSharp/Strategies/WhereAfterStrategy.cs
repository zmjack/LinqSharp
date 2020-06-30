using NStandard;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.Strategies
{
    public class WhereAfterStrategy<TEntity> : IWhereStrategy<TEntity>
    {
        public Expression<Func<TEntity, bool>> StrategyExpression { get; }

        private static readonly MethodInfo _Method_String_Concat = typeof(string).GetMethodViaQualifiedName("System.String Concat(System.Object, System.Object)");
        private static readonly MethodInfo _Method_String_CompareeTo = typeof(string).GetMethodViaQualifiedName("Int32 CompareTo(System.String)");
        private static readonly MethodInfo _Method_DateTime_op_GreaterThanOrEqual = typeof(DateTime).GetMethodViaQualifiedName("Boolean op_GreaterThanOrEqual(System.DateTime, System.DateTime)");
        private static readonly MethodInfo _Method_DateTime_op_GreaterThan = typeof(DateTime).GetMethodViaQualifiedName("Boolean op_GreaterThan(System.DateTime, System.DateTime)");
        private static readonly PropertyInfo _Property_DateTime_HasValue = typeof(DateTime?).GetProperty(nameof(Nullable<DateTime>.HasValue));
        private static readonly PropertyInfo _Property_DateTime_Value = typeof(DateTime?).GetProperty(nameof(Nullable<DateTime>.Value));

        public WhereAfterStrategy(
            Expression<Func<TEntity, DateTime>> memberExp,
            Expression<Func<TEntity, DateTime>> afterExp,
            bool includePoint)
        {
            var left = memberExp.Body;
            var right = afterExp.Body.RebindParameter(afterExp.Parameters[0], memberExp.Parameters[0]);

            StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(includePoint ?
                Expression.GreaterThanOrEqual(left, right, false, _Method_DateTime_op_GreaterThanOrEqual)
                : Expression.GreaterThan(left, right, false, _Method_DateTime_op_GreaterThan), memberExp.Parameters);
        }

        public WhereAfterStrategy(
            Expression<Func<TEntity, DateTime>> memberExp,
            DateTime after,
            bool includePoint)
        {
            var left = memberExp.Body;
            var right = Expression.Constant(after);

            StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(includePoint ?
                Expression.GreaterThanOrEqual(left, right, false, _Method_DateTime_op_GreaterThanOrEqual)
                : Expression.GreaterThan(left, right, false, _Method_DateTime_op_GreaterThan), memberExp.Parameters);
        }

        public WhereAfterStrategy(
            Expression<Func<TEntity, DateTime?>> memberExp,
            Expression<Func<TEntity, DateTime>> afterExp,
            bool liftNullToTrue, bool includePoint)
        {
            var left = memberExp.Body;
            var right = afterExp.Body.RebindParameter(afterExp.Parameters[0], memberExp.Parameters[0]);

            StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Condition(
                    Expression.Not(Expression.Property(memberExp.Body, _Property_DateTime_HasValue)),
                        Expression.Constant(liftNullToTrue),
                        includePoint
                            ? Expression.GreaterThanOrEqual(Expression.Property(left, _Property_DateTime_Value), right, false, _Method_DateTime_op_GreaterThanOrEqual)
                            : Expression.GreaterThan(left, right, false, _Method_DateTime_op_GreaterThan)),
                memberExp.Parameters);
        }

        public WhereAfterStrategy(
            Expression<Func<TEntity, DateTime?>> memberExp,
            DateTime after,
            bool liftNullToTrue, bool includePoint)
        {
            var left = memberExp.Body;
            var right = Expression.Constant(after);

            StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Condition(
                    Expression.Not(Expression.Property(memberExp.Body, _Property_DateTime_HasValue)),
                        Expression.Constant(liftNullToTrue),
                        includePoint
                            ? Expression.GreaterThanOrEqual(Expression.Property(left, _Property_DateTime_Value), right, false, _Method_DateTime_op_GreaterThanOrEqual)
                            : Expression.GreaterThan(left, right, false, _Method_DateTime_op_GreaterThan)),
                memberExp.Parameters);
        }

        public WhereAfterStrategy(
            Expression<Func<TEntity, object>> yearExp,
            Expression<Func<TEntity, object>> monthExp,
            Expression<Func<TEntity, object>> dayExp,
            DateTime before,
            bool includePoint)
        {
            var parameters = yearExp.Parameters;

            Expression GetFilledExpression(Expression bodyExp, int padLength)
            {
                return new int[padLength - 1].Let(i => i).Aggregate(null as Expression, (_acc, i) =>
                {
                    var stringBody = Expression.Convert(bodyExp, typeof(string));
                    return
                        Expression.Condition(
                            Expression.Equal(
                                Expression.Property(stringBody, nameof(string.Length)),
                                Expression.Constant(i)),
                            Expression.Add(
                                Expression.Constant("0".Repeat(padLength - i)),
                                stringBody,
                                _Method_String_Concat),
                            _acc is null ? stringBody : _acc);
                });
            }

            var compareExp =
                Expression.Call(
                    new Expression<Func<TEntity, object>>[]
                    {
                        yearExp,
                        monthExp,
                        dayExp,
                    }.Aggregate(null as Expression, (acc, exp) =>
                    {
                        Expression fullExp;
                        switch (exp)
                        {
                            case Expression<Func<TEntity, object>> e when e == yearExp:
                                fullExp = Expression.Add(
                                    GetFilledExpression(exp.Body, 4),
                                    Expression.Constant("-"), _Method_String_Concat);
                                break;

                            case Expression<Func<TEntity, object>> e when e == monthExp:
                                fullExp = Expression.Add(
                                    GetFilledExpression(exp.Body.RebindParameter(exp.Parameters[0], parameters[0]), 2),
                                    Expression.Constant("-"), _Method_String_Concat);
                                break;

                            case Expression<Func<TEntity, object>> e when e == dayExp:
                                fullExp = GetFilledExpression(exp.Body.RebindParameter(exp.Parameters[0], parameters[0]), 2);
                                break;

                            default: throw new NotSupportedException();
                        }

                        if (acc is null)
                            return fullExp;
                        else return Expression.Add(acc, fullExp, _Method_String_Concat);
                    }),
                    _Method_String_CompareeTo,
                    Expression.Constant(before.ToString("yyyy-MM-dd")));

            StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(includePoint ?
                Expression.GreaterThanOrEqual(compareExp, Expression.Constant(0))
                : Expression.GreaterThan(compareExp, Expression.Constant(0)), parameters);
        }

    }
}
