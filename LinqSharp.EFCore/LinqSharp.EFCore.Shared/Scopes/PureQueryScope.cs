// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Design;
using Microsoft.EntityFrameworkCore;
using NStandard;

namespace LinqSharp.EFCore.Scopes;

public class PureQueryScope : Scope<PureQueryScope>
{
    private readonly List<IDisposable> _scopes = [];

    public PureQueryScope(DbContext context)
    {
        (context as ITimestampable)?.BeginIgnoreTimestamp().Pipe(_scopes.Add);
        (context as IRowLockable)?.BeginIgnoreRowLock().Pipe(_scopes.Add);
        (context as IUserTraceable)?.BeginIgnoreUserTrace().Pipe(_scopes.Add);
    }

    public override void Disposing()
    {
        _scopes.Reverse();
        foreach (var scope in _scopes)
        {
            scope.Dispose();
        }
    }
}
