// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Scopes;
using System.ComponentModel;

namespace LinqSharp.EFCore.Design;

public interface IRowLockable
{
    bool IgnoreRowLock { get; set; }
}

[EditorBrowsable(EditorBrowsableState.Never)]
public static class IRowLockableExtensions
{
    public static IgnoreRowLockScope BeginIgnoreRowLock(this IRowLockable @this)
    {
        return new IgnoreRowLockScope(@this, @this.IgnoreRowLock);
    }
}
