﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Annotations.Params;
using NStandard;

namespace LinqSharp.EFCore.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public sealed class AutoLastWriteTimeAttribute : SpecialAutoAttribute<TimestampParam>
{
    private static readonly Type[] DateTimeTypes = [typeof(DateTime), typeof(DateTime?)];
    private static readonly Type[] DateTimeOffsetTypes = [typeof(DateTimeOffset), typeof(DateTimeOffset?)];

    public AutoLastWriteTimeAttribute() : base(AutoState.Added, AutoState.Modified) { }

    public override object? Format(object entity, Type propertyType, TimestampParam value)
    {
        if (DateTimeTypes.Contains(propertyType)) return value.Now;
        else if (DateTimeOffsetTypes.Contains(propertyType)) return value.NowOffset;
        else throw new ArgumentException($"Only {DateTimeTypes.Join(", ")}, {DateTimeOffsetTypes.Join(", ")} are supported.", nameof(propertyType));
    }
}
