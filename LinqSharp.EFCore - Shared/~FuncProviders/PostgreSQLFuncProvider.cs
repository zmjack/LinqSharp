using Microsoft.EntityFrameworkCore;
using NStandard;
using System;

#pragma warning disable IDE0060 // Remove unused parameter

namespace LinqSharp.EFCore
{
    internal static class PostgreSQLFuncProvider
    {
        public static void Register(ModelBuilder modelBuilder)
        {
            Random(modelBuilder);
            Concat(modelBuilder);
        }

        private static void Random(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDbFunction(typeof(DbFunc).GetMethod(nameof(DbFunc.Random)))
                .HasTranslation(args => Translator.Function("RANDOM", args, typeof(double)));
        }

        private static void Concat(ModelBuilder modelBuilder)
        {
            for (int n = 2; n <= 8; n++)
            {
                var types = new Type[n].Let(() => typeof(string));
                modelBuilder
                    .HasDbFunction(typeof(DbFunc).GetMethod(nameof(DbFunc.Concat), types))
                    .HasTranslation(args => Translator.Function("CONCAT", args, typeof(string)));
            }
        }

    }
}

#pragma warning restore IDE0060 // Remove unused parameter
