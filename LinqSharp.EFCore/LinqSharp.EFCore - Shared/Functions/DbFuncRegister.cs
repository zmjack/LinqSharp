// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
#if !EFCore2
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
#endif
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.EFCore.Functions
{
    public class DbFuncRegister
    {
#if EFCore2
        public delegate Expression TranslatorDelegate(MethodInfo method, Expression[] args);
#else
        public delegate SqlExpression TranslatorDelegate(MethodInfo method, SqlExpression[] args);
#endif
        public ModelBuilder ModelBuilder { get; }

        public DbFuncRegister(ModelBuilder modelBuilder)
        {
            ModelBuilder = modelBuilder;
        }

        public void Register(Expression<Func<object>> dbFunc, TranslatorDelegate register)
        {
            MethodInfo method = null;

            if (dbFunc.Body is UnaryExpression unary)
            {
                if (unary.NodeType == ExpressionType.Convert && unary.Type == typeof(object))
                {
                    if (unary.Operand is MethodCallExpression call) method = call.Method;
                }
            }
            else if (dbFunc.Body is MethodCallExpression call) method = call.Method;
            else throw new ArgumentException(nameof(dbFunc), "Invalid expression.");

            ModelBuilder.HasDbFunction(method).HasTranslation(args => register(method, args.ToArray()));
        }

    }
}
