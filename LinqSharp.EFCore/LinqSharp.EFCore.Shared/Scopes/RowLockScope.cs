// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Design;
using NStandard;

namespace LinqSharp.EFCore.Scopes;

public class RowLockScope : Scope<RowLockScope>
{
    private readonly IRowLockable _context;
    private readonly FieldOption _origin;

    public RowLockScope(IRowLockable context, FieldOption value)
    {
        _context = context;
        _origin = context.RowLockOption;

        context.RowLockOption = value;
    }

    public override void Disposing()
    {
        _context.RowLockOption = _origin;
    }
}
