// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq;

#if EFCORE3_0_OR_GREATER
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
#else
using SqlExpression = System.Linq.Expressions.Expression;
#endif

namespace LinqSharp.EFCore.Translators
{
    public class DbConcat : Translator
    {
        public DbConcat() { }

        public override void RegisterAll(ProviderName providerName, ModelBuilder modelBuilder)
        {
            switch (providerName)
            {
                case ProviderName.MyCat:
                case ProviderName.MySql:
                case ProviderName.PostgreSQL:
                case ProviderName.SqlServer:
                case ProviderName.SqlServerCompact35:
                case ProviderName.SqlServerCompact40:
                    Register_CONCAT(modelBuilder);
                    break;

                case ProviderName.Oracle:
                    Register_Oracle_CONCAT(modelBuilder);
                    break;
            }
        }

        private void Register_CONCAT(ModelBuilder modelBuilder)
        {
            static SqlExpression translator(SqlExpression[] args) => SqlTranslator.Function<string>("CONCAT", args);
            Register(modelBuilder, () => DbFunc.Concat(default, default), translator);
            Register(modelBuilder, () => DbFunc.Concat(default, default, default), translator);
            Register(modelBuilder, () => DbFunc.Concat(default, default, default, default), translator);
            Register(modelBuilder, () => DbFunc.Concat(default, default, default, default, default), translator);
            Register(modelBuilder, () => DbFunc.Concat(default, default, default, default, default, default), translator);
            Register(modelBuilder, () => DbFunc.Concat(default, default, default, default, default, default, default), translator);
            Register(modelBuilder, () => DbFunc.Concat(default, default, default, default, default, default, default, default), translator);
        }

        private void Register_Oracle_CONCAT(ModelBuilder modelBuilder)
        {
            static SqlExpression translator(SqlExpression[] args) => args.Aggregate((a, b) => SqlTranslator.Function<string>("CONCAT", new[] { a, b }));
            Register(modelBuilder, () => DbFunc.Concat(default, default), translator);
            Register(modelBuilder, () => DbFunc.Concat(default, default, default), translator);
            Register(modelBuilder, () => DbFunc.Concat(default, default, default, default), translator);
            Register(modelBuilder, () => DbFunc.Concat(default, default, default, default, default), translator);
            Register(modelBuilder, () => DbFunc.Concat(default, default, default, default, default, default), translator);
            Register(modelBuilder, () => DbFunc.Concat(default, default, default, default, default, default, default), translator);
            Register(modelBuilder, () => DbFunc.Concat(default, default, default, default, default, default, default, default), translator);
        }

    }
}
