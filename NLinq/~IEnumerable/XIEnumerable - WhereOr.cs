using Dawnx.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NLinq
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TSource> WhereOr<TSource>(this IEnumerable<TSource> @this, params Expression<Func<TSource, bool>>[] predicates)
        {
            var parameter = predicates[0].Parameters[0];
            return @this.Where(predicates
                .Select(predicate => predicate.RebindParameter(predicate.Parameters[0], parameter))
                .LambdaJoin(Expression.OrElse).Compile());
        }

        public static IEnumerable<TSource> WhereOrEx<TSource, TAnonymous>(this IEnumerable<TSource> @this, IEnumerable<TAnonymous> anonymousArray)
        {
            var parameter = Expression.Parameter(typeof(TSource));

            var whereExp = anonymousArray.Select(group =>
            {
                var dict = ObjectUtility.GetPropertyPureDictionary(group);
                return dict.Keys.Select(key =>
                {
                    var value = dict[key];

                    var nullable_left = BasicTypeUtility.IsNullableType(Expression.Property(parameter, key).Type);
                    var nullable_right = BasicTypeUtility.IsNullableType(value);

                    if (nullable_left || nullable_right)
                    {
                        return Expression.Lambda<Func<TSource, bool>>(
                            Expression.Equal(
                                Expression.Property(parameter, key),
                                Expression.Convert(Expression.Constant(value), ObjectUtility.GetNullableType(value))),
                            parameter);
                    }
                    else
                    {
                        return Expression.Lambda<Func<TSource, bool>>(
                            Expression.Equal(
                                Expression.Property(parameter, key),
                                Expression.Constant(value)),
                            parameter);
                    }
                }).LambdaJoin(Expression.AndAlso);
            }).LambdaJoin(Expression.OrElse);

            return @this.Where(whereExp.Compile());
        }

    }
}