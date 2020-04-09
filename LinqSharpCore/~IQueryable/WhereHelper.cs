using LinqSharp.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public class WhereHelper<TSource>
    {
        public IQueryable<TSource> Sources;

        public WhereHelper(IQueryable<TSource> sources)
        {
            Sources = sources;
        }

        private Expression<Func<TSource, bool>> ParsePredicate<T>(Expression<Func<TSource, T, bool>> exp)
        {
            //static Expression parse(Expression _exp, Type inputType)
            //{
            //    if (_exp is BinaryExpression binExp)
            //    {
            //        var left = parse(binExp.Left, inputType);
            //        var right = parse(binExp.Right, inputType);
            //        return _exp.NodeType switch
            //        {
            //            ExpressionType.AndAlso => Expression.AndAlso(left, right),
            //            ExpressionType.OrElse => Expression.OrElse(left, right),
            //            ExpressionType.Equal => Expression.Equal(left, right),
            //            _ => throw new NotSupportedException(),
            //        };
            //    }
            //    else if (_exp is MemberExpression memExp)
            //    {
            //        if (memExp.Expression.Type == inputType)
            //        {
            //            Expression.Lambda <>
            //            throw new NotSupportedException();
            //        }
            //        else return memExp;
            //    }
            //    else throw new NotSupportedException();
            //}

            //parse(exp.Body, exp.Parameters[1].Type);
            throw new NotSupportedException();

            //if (exp.Body is BinaryExpression binExp)
            //{
            //    binExp.
            //}
        }

        public WhereExp<TSource> And(params WhereExp<TSource>[] whereExps) => whereExps.Aggregate((x, y) => x & y);
        public WhereExp<TSource> And<T>(IEnumerable<T> enumerable, Expression<Func<TSource, bool>> exp)
        {
            var whereExps = enumerable.Select(e => new WhereExp<TSource>(exp)).ToArray();
            return And(whereExps);
        }
        public WhereExp<TSource> Or(params WhereExp<TSource>[] whereExps) => whereExps.Aggregate((x, y) => x | y);
        public WhereExp<TSource> Or<T>(IEnumerable<T> enumerable, Func<T, Expression<Func<TSource, bool>>> exp)
        {
            var whereExps = enumerable.Select(e => new WhereExp<TSource>(exp(e))).ToArray();
            return Or(whereExps);
        }

        public WhereExp<TSource> Where(Expression<Func<TSource, bool>> selector) => new WhereExp<TSource>(selector);
        public WhereExp<TSource> WhereDynamic(Action<DynamicExpressionBuilder<TSource>> build)
        {
            var builder = new DynamicExpressionBuilder<TSource>();
            build(builder);
            return new WhereExp<TSource>(builder.Lambda);
        }

        public WhereExp<TSource> WhereMin<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            if (Sources.Any())
            {
                var min = Sources.Min(selector);
                var whereExp = Expression.Lambda<Func<TSource, bool>>(
                    Expression.Equal(selector.Body, Expression.Constant(min, typeof(TResult))), selector.Parameters);
                return new WhereExp<TSource>(whereExp);
            }
            else return new WhereExp<TSource>(x => false);
        }
        public WhereExp<TSource> WhereMax<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            if (Sources.Any())
            {
                var min = Sources.Max(selector);
                var whereExp = Expression.Lambda<Func<TSource, bool>>(
                    Expression.Equal(selector.Body, Expression.Constant(min, typeof(TResult))), selector.Parameters);
                return new WhereExp<TSource>(whereExp);
            }
            else return new WhereExp<TSource>(x => false);
        }

        public WhereExp<TSource> WhereSearch(string searchString, Expression<Func<TSource, object>> searchMembers)
        {
            var strategy = new WhereSearchStrategy<TSource>(searchString, searchMembers);
            return new WhereExp<TSource>(strategy.StrategyExpression);
        }
        public WhereExp<TSource> WhereMatch(string searchString, Expression<Func<TSource, object>> searchMembers)
        {
            var strategy = new WhereMatchStrategy<TSource>(searchString, searchMembers);
            return new WhereExp<TSource>(strategy.StrategyExpression);
        }

    }

}
