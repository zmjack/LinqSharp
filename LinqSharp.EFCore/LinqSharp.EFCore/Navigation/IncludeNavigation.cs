// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LinqSharp.EFCore.Navigation
{
    public class IncludeNavigation<TDbContext, TEntity, TProperty> : IIncludable<TDbContext, TEntity, TProperty>
        where TDbContext : DbContext
        where TEntity : class
        where TProperty : class
    {
        public CompoundQuery<TDbContext, TEntity> Owner { get; }
        public List<QueryTarget> TargetPath { get; }

        internal IncludeNavigation(CompoundQuery<TDbContext, TEntity> owner, List<QueryTarget> targetPath)
        {
            Owner = owner;
            TargetPath = targetPath;
        }
    }

}
