// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;

namespace LinqSharp.EFCore.Functions.Providers
{
    public class JetFuncProvider : DbFuncProvider
    {
        public JetFuncProvider(ModelBuilder modelBuilder) : base(modelBuilder) { }

        public override void UseRandom()
        {
            _register.Register(() => DbFunc.Random(), (method, args) => Translator.Function<double>("RND", args));
        }

        public override void UseConcat()
        {
        }

        public override void UseDateTime()
        {
        }

    }
}
