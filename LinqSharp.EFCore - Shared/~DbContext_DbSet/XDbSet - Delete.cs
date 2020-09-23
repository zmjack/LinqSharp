// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.EFCore
{
    public static partial class XDbSet
    {
        public static void Delete<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var records = @this.Where(predicate);
            @this.RemoveRange(records);
        }

    }
}
