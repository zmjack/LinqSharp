// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
#if !EFCore2
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
#endif
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.EFCore.Functions.Providers
{
    public class OracleFuncProvider : DbFuncProvider
    {
        public OracleFuncProvider(ModelBuilder modelBuilder) : base(modelBuilder) { }

        public override void UseRandom()
        {
            _register.Register(() => DbFunc.Random(), (method, args) => Translator.Function<double>("DBMS_RANDOM", "RANDOM", args));
        }

        public override void UseConcat()
        {
#if EFCore2
            static Expression translator(MethodInfo method, Expression[] args) => args.Aggregate((a, b) => Translator.Function<string>("CONCAT", new[] { a, b }));
#else
            static SqlExpression translator(MethodInfo method, SqlExpression[] args) => args.Aggregate((a, b) => Translator.Function<string>("CONCAT", new[] { a, b }));
#endif
            _register.Register(() => DbFunc.Concat(default, default), translator);
            _register.Register(() => DbFunc.Concat(default, default, default), translator);
            _register.Register(() => DbFunc.Concat(default, default, default, default), translator);
            _register.Register(() => DbFunc.Concat(default, default, default, default, default), translator);
            _register.Register(() => DbFunc.Concat(default, default, default, default, default, default), translator);
            _register.Register(() => DbFunc.Concat(default, default, default, default, default, default, default), translator);
            _register.Register(() => DbFunc.Concat(default, default, default, default, default, default, default, default), translator);
        }

        public override void UseDateTime()
        {
        }

    }
}
