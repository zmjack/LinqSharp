// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
#if EFCORE3_0_OR_GREATER
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
#endif

namespace LinqSharp.EFCore.Functions
{
    public abstract class DbFuncProvider : IDbFuncProvider
    {
        protected readonly DbFuncRegister _register;

        public DbFuncProvider(ModelBuilder modelBuilder)
        {
            _register = new DbFuncRegister(modelBuilder);
        }

        public void UseAll()
        {
            UseRandom();
            UseConcat();
            UseDateTime();
            UseToDouble();
        }

        public virtual void UseRandom() { }
        public virtual void UseConcat() { }
        public virtual void UseDateTime() { }
        public virtual void UseToDouble() { }

    }
}
