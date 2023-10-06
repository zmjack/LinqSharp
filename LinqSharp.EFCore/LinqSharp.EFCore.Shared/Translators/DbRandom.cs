// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Query;
using Microsoft.EntityFrameworkCore;
using System;

namespace LinqSharp.EFCore.Translators;

public class DbRandom : Translator
{
    private static readonly Random RandomInstance = new();
    public static double NextDouble() => RandomInstance.NextDouble();

    public DbRandom() { }

    public override void RegisterAll(ProviderName providerName, ModelBuilder modelBuilder)
    {
        switch (providerName)
        {
            case ProviderName.Jet:
                Register_RND(modelBuilder);
                break;

            case ProviderName.MyCat:
            case ProviderName.MySql:
            case ProviderName.SqlServer:
            case ProviderName.SqlServerCompact35:
            case ProviderName.SqlServerCompact40:
                Register_RAND(modelBuilder);
                break;

            case ProviderName.PostgreSQL:
            case ProviderName.Sqlite:
                Register_RANDOM(modelBuilder);
                break;

            case ProviderName.Oracle:
                Register_Oracle_RANDOM(modelBuilder);
                break;
        }
    }

    private void Register_RND(ModelBuilder modelBuilder)
    {
        Register(modelBuilder, () => NextDouble(), args => SqlTranslator.Function<double>("RND", args));
    }

    private void Register_RAND(ModelBuilder modelBuilder)
    {
        Register(modelBuilder, () => NextDouble(), args => SqlTranslator.Function<double>("RAND", args));
    }

    private void Register_RANDOM(ModelBuilder modelBuilder)
    {
        Register(modelBuilder, () => NextDouble(), args => SqlTranslator.Function<double>("RANDOM", args));
    }

    private void Register_Oracle_RANDOM(ModelBuilder modelBuilder)
    {
        Register(modelBuilder, () => NextDouble(), args => SqlTranslator.Function<double>("DBMS_RANDOM", "RANDOM", args));
    }
}
