// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Query;
using Microsoft.EntityFrameworkCore;
using System;

#if EFCORE3_1_OR_GREATER
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
#else
using SqlExpression = System.Linq.Expressions.Expression;
#endif

namespace LinqSharp.EFCore.Translators
{
    public class DbDateTime : Translator
    {
        public static DateTime Create(int year, int month, int day) => new(year, month, day);
        public static DateTime Create(int year, int month, int day, int hour, int minute, int second) => new(year, month, day, hour, minute, second);

        public DbDateTime() { }

        public override void RegisterAll(ProviderName providerName, ModelBuilder modelBuilder)
        {
            switch (providerName)
            {
                case ProviderName.MyCat:
                case ProviderName.MySql:
                    Register_MySql(modelBuilder);
                    break;

                case ProviderName.SqlServer:
                case ProviderName.SqlServerCompact35:
                case ProviderName.SqlServerCompact40:
                    Register_SqlServer(modelBuilder);
                    break;
            }
        }

        private void Register_MySql(ModelBuilder modelBuilder)
        {
            Register(modelBuilder, () => Create(default, default, default), args =>
            {
                var hyphen = SqlTranslator.Constant("-");
                return
                    SqlTranslator.Function<DateTime>("STR_TO_DATE",
                        SqlTranslator.Function<string>("CONCAT", args[0], hyphen, args[1], hyphen, args[2]),
                        SqlTranslator.Constant("%Y-%m-%d"));
            });

            Register(modelBuilder, () => Create(default, default, default, default, default, default), args =>
            {
                var hyphen = SqlTranslator.Constant("-");
                var space = SqlTranslator.Constant(" ");
                var colon = SqlTranslator.Constant(":");
                return
                    SqlTranslator.Function<DateTime>("STR_TO_DATE",
                        SqlTranslator.Function<string>("CONCAT",
                            SqlTranslator.Function<string>("CONCAT", args[0], hyphen, args[1], hyphen, args[2]),
                            SqlTranslator.Function<string>("CONCAT", space, args[3], colon, args[4], colon, args[5])),
                        SqlTranslator.Constant("%Y-%m-%d %H:%i:%s"));
            });
        }

        private void Register_SqlServer(ModelBuilder modelBuilder)
        {
            Register(modelBuilder, () => Create(default, default, default), args =>
            {
                var hyphen = SqlTranslator.Constant("-");
                return
                    SqlTranslator.Function<DateTime>("CONVERT",
                        SqlTranslator.Fragment("DATETIME"),
                        SqlTranslator.Function<string>("CONCAT", args[0], hyphen, args[1], hyphen, args[2]));
            });

            Register(modelBuilder, () => Create(default, default, default, default, default, default), args =>
            {
                var hyphen = SqlTranslator.Constant("-");
                var space = SqlTranslator.Constant(" ");
                var colon = SqlTranslator.Constant(":");
                return
                    SqlTranslator.Function<DateTime>("CONVERT",
                        SqlTranslator.Fragment("DATETIME"),
                        SqlTranslator.Function<string>("CONCAT",
                            SqlTranslator.Function<string>("CONCAT", args[0], hyphen, args[1], hyphen, args[2]),
                            SqlTranslator.Function<string>("CONCAT", space, args[3], colon, args[4], colon, args[5])));
            });
        }

    }
}
