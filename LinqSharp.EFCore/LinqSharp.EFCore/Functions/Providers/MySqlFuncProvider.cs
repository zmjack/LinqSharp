// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Query;
using Microsoft.EntityFrameworkCore;
#if EFCORE3_0_OR_GREATER
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
#endif
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.EFCore.Functions.Providers
{
    public class MySqlFuncProvider : DbFuncProvider
    {
        public MySqlFuncProvider(ModelBuilder modelBuilder) : base(modelBuilder) { }

        public override void UseRandom()
        {
            Register(() => DbFunc.Random(), (method, args) => Translator.Function<double>("RAND", args));
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
            Register(() => DbFunc.DateTime(default, default, default), (method, args) =>
            {
                var hyphen = Translator.Constant("-");
                return
                    Translator.Function<DateTime>("STR_TO_DATE",
                        Translator.Function<string>("CONCAT", args[0], hyphen, args[1], hyphen, args[2]),
                        Translator.Constant("%Y-%m-%d"));
            });

            Register(() => DbFunc.DateTime(default, default, default, default, default, default), (method, args) =>
            {
                var hyphen = Translator.Constant("-");
                var space = Translator.Constant(" ");
                var colon = Translator.Constant(":");
                return
                    Translator.Function<DateTime>("STR_TO_DATE",
                        Translator.Function<string>("CONCAT",
                            Translator.Function<string>("CONCAT", args[0], hyphen, args[1], hyphen, args[2]),
                            Translator.Function<string>("CONCAT", space, args[3], colon, args[4], colon, args[5])),
                        Translator.Constant("%Y-%m-%d %H:%i:%s"));
            });
        }

        public override void UseToDouble()
        {
            Register(() => DbFunc.ToDouble(default), (method, args) =>
            {
                return Translator.Function<double>("CONVERT", args[0], Translator.Fragment("DECIMAL(16, 4)"));
            });
        }

    }
}
