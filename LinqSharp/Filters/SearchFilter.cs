using LinqSharp.Design;
using NStandard;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.Filters;

internal class SearchFilter<T> : IFieldFilter<T>
{
    private readonly string[] _searches;
    private readonly Expression<Func<T, SearchSelector>> _selector;
    private readonly MethodInfo _search;
    private readonly bool _useNot;

    public SearchFilter(SearchMode mode, string[] searches, Expression<Func<T, SearchSelector>> selector)
    {
        _searches = [.. from s in searches where !string.IsNullOrEmpty(s) select s];
        if (_searches.Length == 0) throw new ArgumentException("Search content cannot be empty.", nameof(searches));

        if (selector.Body.NodeType != ExpressionType.ListInit) throw new ArgumentException("Expression must be new SearchSelector() {...} .", nameof(selector));

        var root = selector.Body as ListInitExpression;
        if (root.NewExpression.Type != typeof(SearchSelector)) throw new ArgumentException("Expression must be new SearchSelector() {...} .", nameof(selector));
        _selector = selector;

        _search = mode switch
        {
            SearchMode.Default => MethodAccessor.String.Contains,
            SearchMode.Contains => MethodAccessor.String.Contains,
            SearchMode.Equals => MethodAccessor.String.Equals,
            SearchMode.NotContains => MethodAccessor.String.Contains,
            SearchMode.NotEquals => MethodAccessor.String.Equals,
            _ => throw new NotImplementedException(),
        };
        _useNot = mode switch
        {
            SearchMode.Default => false,
            SearchMode.Contains => false,
            SearchMode.Equals => false,
            SearchMode.NotContains => true,
            SearchMode.NotEquals => true,
            _ => throw new NotImplementedException(),
        };
    }

    private static MethodCallExpression RebuildMethodCall(MethodCallExpression call, ParameterExpression param)
    {
        var firstArgument = call.Arguments[0];
        if (firstArgument is MemberExpression member)
        {
            var leftSub = member.RebindNode(member.Expression, param);
            return Expression.Call(call.Method, leftSub, call.Arguments[1]);
        }
        else if (firstArgument is MethodCallExpression subCall)
        {
            var sub = new Expression[subCall.Arguments.Count];
            sub[0] = RebuildMethodCall(subCall, param);
            for (int i = 1; i < sub.Length; i++)
            {
                sub[i] = call.Arguments[i];
            }

            return Expression.Call(call.Method, sub[0], sub[1]);
        }
        else throw new NotImplementedException();
    }

    public QueryExpression<T> Filter(QueryHelper<T> h)
    {
        ConstantExpression[] searchConstants =
        [
            ..
            from s in _searches
            select Expression.Constant(s, typeof(string))
        ];

        Expression BuildSearch(Expression field)
        {
            var type = field switch
            {
                MemberExpression member => member.Type,
                ParameterExpression parameter => parameter.Type,
                _ => throw new ArgumentException("Only MemberExpression or ParameterExpression is supported.", nameof(field)),
            };
            var needConvert = type != typeof(string);
            var nullConstant = type switch
            {
                Type when type.IsValueType => type.IsNullableValue() ? Expression.Constant(null, type) : null,
                _ => Expression.Constant(null, type),
            };

            Expression Clause(ConstantExpression search)
            {
                return needConvert
                    ? Expression.Call(Expression.Call(field, MethodAccessor.Object.ToStringMethod), _search, search)
                    : Expression.Call(field, _search, search);
            }

            Expression CheckNullClause(Expression clause)
            {
                if (nullConstant is not null)
                {
                    return Expression.AndAlso(
                        Expression.NotEqual(field, nullConstant),
                        clause
                    );
                }
                else return clause;
            }

            if (searchConstants.Length > 1)
            {
                return CheckNullClause(
                (
                    from exp in searchConstants
                    select Clause(exp)
                ).ExpressionJoin(Expression.OrElse)!);
            }
            else if (searchConstants.Length == 1)
            {
                return CheckNullClause(
                    Clause(searchConstants[0])
                );
            }
            else throw new ArgumentException("No search text specified.");
        }

        var root = (_selector.Body as ListInitExpression)!;
        var list = new List<Expression>();

        foreach (var initializer in root.Initializers)
        {
            var expression = initializer.Arguments[0];
            if (expression is MemberExpression member)
            {
                if (member.Expression is not ParameterExpression) throw new ArgumentException($"Only parameter's member can be supported. {expression}");

                var sub = member.RebindNode(member.Expression, h.PropertyParameter);
                list.Add(BuildSearch(sub));
            }
            else if (expression is UnaryExpression unary)
            {
                if (unary.NodeType == ExpressionType.Convert)
                {
                    var _member = (unary.Operand as MemberExpression)!;
                    if (_member.Expression is not ParameterExpression) throw new ArgumentException($"Only parameter's member can be supported. {expression}");

                    var sub = _member.RebindNode(_member.Expression, h.PropertyParameter);
                    list.Add(BuildSearch(sub));
                }
            }
            else if (expression is NewExpression @new)
            {
                foreach (var part in @new.Arguments.OfType<MemberExpression>())
                {
                    if (part.Expression is not ParameterExpression) throw new ArgumentException($"Only parameter's member can be supported. {expression}");

                    var sub = part.RebindNode(part.Expression, h.PropertyParameter);
                    list.Add(BuildSearch(sub));
                }
            }
            else if (expression is MethodCallExpression call)
            {
                var sub = RebuildMethodCall(call, h.PropertyParameter);

                if (sub.Type.IsImplement(typeof(IEnumerable)))
                {
                    var selectType = sub.Type.GetGenericArguments()[0];

                    if (selectType.IsAnonymousType())
                    {
                        if (sub.Arguments[1] is LambdaExpression lambda)
                        {
                            var param = lambda.Parameters[0];
                            if (lambda.Body is NewExpression lambdaNew)
                            {
                                Expression? innerExp = null;
                                foreach (var part in lambdaNew.Arguments.OfType<MemberExpression>())
                                {
                                    if (innerExp is null)
                                    {
                                        innerExp = BuildSearch(part);
                                    }
                                    else
                                    {
                                        innerExp = Expression.OrElse(
                                            innerExp,
                                            BuildSearch(part)
                                        );
                                    }
                                }

                                var elementType = sub.Arguments[0].Type.GetGenericArguments()[0];
                                list.Add(
                                    Expression.Call(
                                        MethodAccessor.Enumerable.Any1.MakeGenericMethod(elementType),
                                        sub.Arguments[0],
                                        Expression.Lambda(innerExp, param))
                                );
                            }
                        }
                    }
                    else
                    {
                        var element = Expression.Parameter(selectType);
                        list.Add(
                            Expression.Call(
                                MethodAccessor.Enumerable.Any1.MakeGenericMethod(selectType),
                                sub,
                                Expression.Lambda(
                                    BuildSearch(element),
                                    element
                                )
                            )
                        );
                    }
                }
            }
            else throw new NotImplementedException();
        }

        if (_useNot)
        {
            return h.And(
                from lambda in list
                select Expression.Lambda<Func<T, bool>>(Expression.Not(lambda), [h.PropertyParameter])
            );
        }
        else
        {
            return h.Or(
                from lambda in list
                select Expression.Lambda<Func<T, bool>>(lambda, [h.PropertyParameter])
            );
        }
    }
}
