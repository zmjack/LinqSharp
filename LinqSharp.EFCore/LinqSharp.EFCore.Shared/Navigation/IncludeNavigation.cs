// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Scopes;

namespace LinqSharp.EFCore.Navigation;

public class IncludeNavigation<TEntity, TProperty> : IIncludable<TEntity, TProperty>
    where TEntity : class
    where TProperty : class
{
    public CompoundQuery<TEntity> Owner { get; }
    public List<QueryTarget> TargetPath { get; }

    internal IncludeNavigation(CompoundQuery<TEntity> owner, List<QueryTarget> targetPath)
    {
        Owner = owner;
        TargetPath = targetPath;
    }
}
