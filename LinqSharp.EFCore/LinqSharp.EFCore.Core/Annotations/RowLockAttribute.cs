// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Annotations.Params;
using System;

namespace LinqSharp.EFCore.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public sealed class RowLockAttribute : SpecialAutoAttribute<LockParam>
{
    public RowLockAttribute() : base(AutoState.Modified, AutoState.Deleted) { }

    public override object Format(object entity, Type propertyType, LockParam value)
    {
        if (value.IgnoreRowLock) return value.Current;
        if (value.Origin is null) return value.Current;

        throw new InvalidOperationException("Must be unlocked before updating records.");
    }
}
