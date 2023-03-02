// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Scopes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LinqSharp.EFCore.Navigation
{
    public interface IIncludable<TEntity, out TProperty>
        where TEntity : class
    {
        CompoundQuery<TEntity> Owner { get; }
        List<QueryTarget> TargetPath { get; }
    }
}
