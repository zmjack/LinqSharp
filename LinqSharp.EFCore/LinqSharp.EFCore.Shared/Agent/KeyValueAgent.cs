// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Entities;

namespace LinqSharp.EFCore.Agent;

/// <summary>
/// Hint: Each custom properties must be virtual(public).
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class KeyValueAgent<TEntity>
    where TEntity : KeyValueEntity, new()
{
#pragma warning disable IDE1006 // Naming Styles
    public string __ItemName__ { get; internal set; }
#pragma warning restore IDE1006 // Naming Styles

    internal bool _executed;
    internal KeyValueEntity[] _entities;

}
