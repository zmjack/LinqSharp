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

    public PureQueryScope(DbContext context, FieldOption option)
    {
        (context as ITimestampable)?.BeginTimestamp(option).Pipe(_scopes.Add);
        (context as IRowLockable)?.BeginRowLock(option).Pipe(_scopes.Add);
        (context as IUserTraceable)?.BeginUserTrace(option).Pipe(_scopes.Add);
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
