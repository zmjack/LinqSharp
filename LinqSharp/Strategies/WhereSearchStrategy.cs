using NStandard;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.Strategies
{
    public class WhereSearchStrategy<TEntity> : WhereStringStrategy<TEntity>
    {
        private static Func<Expression, Expression, Expression> _CompareExp;

        static WhereSearchStrategy()
        {
            _CompareExp = (singlePartOfInExp, secharStringExp) => singlePartOfInExp.For(_ =>
            {
                if (_.Type == typeof(string))
                    return Expression.Call(singlePartOfInExp, typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) }), secharStringExp);
                else if (_.Type.GetInterface(typeof(IEnumerable).FullName) != null)
                {
                    var parameter = Expression.Parameter(typeof(string));
                    var anyMethod = typeof(Enumerable)
                        .GetMethodViaQualifiedName("Boolean Any[TSource](System.Collections.Generic.IEnumerable`1[TSource], System.Func`2[TSource,System.Boolean])")
                        .MakeGenericMethod(typeof(string));
                    var lambda = Expression.Lambda<Func<string, bool>>(
                        Expression.Call(parameter, typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) }), secharStringExp),
                        parameter);

                    return Expression.Call(anyMethod, singlePartOfInExp, lambda);
                }
                else throw new NotSupportedException();
            });
        }

        public WhereSearchStrategy(string searchString, Expression<Func<TEntity, object>> searchMembers)
            : base(searchMembers, _CompareExp, searchString ?? "")
        {
        }

    }
}
