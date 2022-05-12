// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static LinqSharp.EFCore.Functions.DbFuncRegister;

#if EFCORE6_0_OR_GREATER
#elif EFCORE3_0_OR_GREATER
using System;
#elif EFCORE2_0_OR_GREATER
using System;
#endif

namespace LinqSharp.EFCore.Functions
{
    public abstract class DbFuncProvider : IDbFuncProvider
    {
        private readonly DbFuncRegister _register;

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

        public void Register(Expression<Func<object>> dbFunc, TranslatorDelegate register)
        {
            _register.Register(dbFunc, register);
        }
    }
}
