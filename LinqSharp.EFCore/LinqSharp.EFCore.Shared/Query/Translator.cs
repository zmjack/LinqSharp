// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
#if EFCORE3_1_OR_GREATER
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
#else
#endif

#if EFCORE3_1_OR_GREATER
#else
using SqlExpression = System.Linq.Expressions.Expression;
#endif

namespace LinqSharp.EFCore.Query;

public abstract class Translator
{
    protected static NotSupportedException CannotBeCalled() => new($"This method does not support direct evaluation.");

    public Translator()
    {
    }

    public abstract void RegisterAll(ProviderName providerName, ModelBuilder modelBuilder);

    public void Register(ModelBuilder modelBuilder, Expression<Func<object>> methodGetter, Func<SqlExpression[], SqlExpression> build)
    {
        MethodInfo? method = null;

        if (methodGetter.Body is UnaryExpression unary)
        {
            if (unary.NodeType == ExpressionType.Convert && unary.Type == typeof(object))
            {
                if (unary.Operand is MethodCallExpression call) method = call.Method;
            }
        }
        else if (methodGetter.Body is MethodCallExpression call) method = call.Method;

        if (method is null) throw new ArgumentException("Invalid expression.", nameof(methodGetter));
        if (!method.IsStatic) throw new ArgumentException("The registration method must be a static method.", nameof(methodGetter));

        modelBuilder.HasDbFunction(method).HasTranslation(args => build(args.ToArray()));
    }
}
