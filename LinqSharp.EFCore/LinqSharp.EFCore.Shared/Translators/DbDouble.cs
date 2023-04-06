// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Query;
using Microsoft.EntityFrameworkCore;

#if EFCORE3_1_OR_GREATER
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
#else
using SqlExpression = System.Linq.Expressions.Expression;
#endif

namespace LinqSharp.EFCore.Translators
{
    public class DbDouble : Translator
    {
        public static double FromInt(int number) => number;

        public DbDouble() { }

        public override void RegisterAll(ProviderName providerName, ModelBuilder modelBuilder)
        {
            switch (providerName)
            {
                case ProviderName.MyCat:
                case ProviderName.MySql:
                    MySqlRegister(this, modelBuilder);
                    break;
            }
        }

        private void MySqlRegister(Translator provider, ModelBuilder modelBuilder)
        {
            provider.Register(modelBuilder, () => FromInt(default), args =>
            {
                return SqlTranslator.Function<double>("CONVERT", args[0], SqlTranslator.Fragment("DECIMAL(16, 4)"));
            });
        }

    }
}
