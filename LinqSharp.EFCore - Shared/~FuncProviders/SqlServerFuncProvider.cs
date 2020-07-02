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
                .HasTranslation(args => Translator.Function("RAND", args, typeof(double)));
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

        private static void DateTime(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDbFunction(typeof(DbFunc).GetMethod(nameof(DbFunc.DateTime), new[] { typeof(int), typeof(int), typeof(int) }))
                .HasTranslation(args =>
                {
                    var line = Translator.Constant("-");
                    var _args = args.ToArray();
                    return
                        Translator.Function("CONVERT",
                            Translator.Concat(
                                Translator.Fragment("DATETIME"),
                                Translator.Function("CONCAT", new[] { _args[0], line, _args[1], line, _args[2] }, typeof(string))),
                            typeof(DateTime));
                });

            modelBuilder
                .HasDbFunction(typeof(DbFunc).GetMethod(nameof(DbFunc.DateTime), new[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) }))
                .HasTranslation(args =>
                {
                    var line = Translator.Constant("-");
                    var space = Translator.Constant(" ");
                    var colon = Translator.Constant(":");
                    var _args = args.ToArray();
                    return
                        Translator.Function("CONVERT",
                            Translator.Concat(
                                Translator.Fragment("DATETIME"),
                                Translator.Function("CONCAT", new[] { _args[0], line, _args[1], line, _args[2], space, _args[3], colon, _args[4], colon, _args[5] }, typeof(string))),
                            typeof(DateTime));
                });
        }

    }
}

#pragma warning restore IDE0060 // Remove unused parameter
