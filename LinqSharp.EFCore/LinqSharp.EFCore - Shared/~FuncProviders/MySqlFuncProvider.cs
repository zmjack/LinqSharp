// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Linq;

namespace LinqSharp.EFCore
{
    internal static class MySqlFuncProvider
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
            for (int n = 2; n <= 8; n++)
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
                    var _args = args.ToArray();
                    var hyphen = Translator.Constant("-");
                    return
                        Translator.Function<DateTime>("STR_TO_DATE",
                            Translator.Function<string>("CONCAT", new[] { _args[0], hyphen, _args[1], hyphen, _args[2] }),
                            Translator.Constant("%Y-%m-%d"));
                });

            modelBuilder
                .HasDbFunction(typeof(DbFunc).GetMethod(nameof(DbFunc.DateTime), new[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) }))
                .HasTranslation(args =>
                {
                    var _args = args.ToArray();
                    var hyphen = Translator.Constant("-");
                    var space = Translator.Constant(" ");
                    var colon = Translator.Constant(":");
                    return
                        Translator.Function<DateTime>("STR_TO_DATE",
                            Translator.Function<string>("CONCAT", new[] { _args[0], hyphen, _args[1], hyphen, _args[2], space, _args[3], colon, _args[4], colon, _args[5] }),
                            Translator.Constant("%Y-%m-%d %H:%i:%s"));
                });
        }

    }
}
