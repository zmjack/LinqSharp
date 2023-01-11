// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Query;
using Microsoft.EntityFrameworkCore;

namespace LinqSharp.EFCore.Functions.Providers
{
    public class SqliteFuncProvider : DbFuncProvider
    {
        public SqliteFuncProvider(ModelBuilder modelBuilder) : base(modelBuilder) { }

        public override void UseRandom()
        {
            Register(() => DbFunc.Random(), (method, args) => Translator.Function<double>("RANDOM", args));
        }

        public override void UseConcat()
        {
        }

        public override void UseDateTime()
        {
        }

    }
}
