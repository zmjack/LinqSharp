// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Linq;

#pragma warning disable IDE0060 // Remove unused parameter

namespace LinqSharp.EFCore
{
    internal static class SqlServerFuncProvider
    {
        public static void Register(ModelBuilder modelBuilder)
        {
            Random(modelBuilder);
            Concat(modelBuilder);
            DateTime(modelBuilder);
        }

        private static void Random(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDbFunction(typeof(DbFunc).GetMethod(nameof(DbFunc.Random)))
                .HasTranslation(args => Translator.Function<double>("RAND", args));
        }

        private static void Concat(ModelBuilder modelBuilder)
        {
            for (int n = DbFunc.MinConcat; n <= DbFunc.MaxConcat; n++)
            {
                var types = new Type[n].Let(() => typeof(string));
                modelBuilder
                    .HasDbFunction(typeof(DbFunc).GetMethod(nameof(DbFunc.Concat), types))
                    .HasTranslation(args => Translator.Function<string>("CONCAT", args));
            }
        }

        private static void DateTime(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDbFunction(typeof(DbFunc).GetMethod(nameof(DbFunc.DateTime), new[] { typeof(int), typeof(int), typeof(int) }))
                .HasTranslation(args =>
                {
                    var hyphen = Translator.Constant("-");
                    var _args = args.ToArray();
                    return
                        Translator.Function<DateTime>("CONVERT",
                            Translator.Fragment("DATETIME"),
                            Translator.Function<string>("CONCAT", new[] { _args[0], hyphen, _args[1], hyphen, _args[2] }));
                });

            modelBuilder
                .HasDbFunction(typeof(DbFunc).GetMethod(nameof(DbFunc.DateTime), new[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) }))
                .HasTranslation(args =>
                {
                    var hyphen = Translator.Constant("-");
                    var colon = Translator.Constant(":");
                    var _args = args.ToArray();
                    return
                        Translator.Function<DateTime>("CONVERT",
                            Translator.Fragment("DATETIME"),
                            Translator.Function<string>("CONCAT", new[] { _args[0], hyphen, _args[1], hyphen, _args[2], Translator.Constant(" "), _args[3], colon, _args[4], colon, _args[5] }));
                });
        }

    }
}

#pragma warning restore IDE0060 // Remove unused parameter
