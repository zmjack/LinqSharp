// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
#if EFCORE3_0_OR_GREATER
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
#endif
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.EFCore.Functions.Providers
{
    public class PostgreSQLFuncProvider : DbFuncProvider
    {
        public PostgreSQLFuncProvider(ModelBuilder modelBuilder) : base(modelBuilder) { }

        public override void UseRandom()
        {
            Register(() => DbFunc.Random(), (method, args) => Translator.Function<double>("RANDOM", args));
        }

        public override void UseConcat()
        {
#if EFCORE3_0_OR_GREATER
            static SqlExpression translator(MethodInfo method, SqlExpression[] args) => Translator.Function<string>("CONCAT", args);
#else
            static Expression translator(MethodInfo method, Expression[] args) => Translator.Function<string>("CONCAT", args);
#endif
            Register(() => DbFunc.Concat(default, default), translator);
            Register(() => DbFunc.Concat(default, default, default), translator);
            Register(() => DbFunc.Concat(default, default, default, default), translator);
            Register(() => DbFunc.Concat(default, default, default, default, default), translator);
            Register(() => DbFunc.Concat(default, default, default, default, default, default), translator);
            Register(() => DbFunc.Concat(default, default, default, default, default, default, default), translator);
            Register(() => DbFunc.Concat(default, default, default, default, default, default, default, default), translator);
        }

        public override void UseDateTime()
        {
        }

    }
}
