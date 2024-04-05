// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;

namespace LinqSharp.EFCore.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public sealed class AutoMonthOnlyAttribute : AutoAttribute
{
    public AutoMonthOnlyAttribute() : base(AutoState.Added, AutoState.Modified) { }
    public override object? Format(object entity, Type propertyType, object? value)
    {
        if (propertyType == typeof(DateTime)) return ((DateTime)value!).StartOfMonth();
        if (propertyType == typeof(DateTime?)) return ((DateTime?)value)?.StartOfMonth();

        if (propertyType == typeof(DateTimeOffset)) return ((DateTimeOffset)value!).StartOfMonth();
        if (propertyType == typeof(DateTimeOffset?)) return ((DateTimeOffset?)value)?.StartOfMonth();

#if NET6_0_OR_GREATER
        if (propertyType == typeof(DateOnly)) return ((DateOnly)value!).StartOfMonth();
        if (propertyType == typeof(DateOnly?)) return ((DateOnly?)value)?.StartOfMonth();

        throw new ArgumentException($"Only DateTime, DateTimeOffset, DateOnly are supported.", nameof(propertyType));
#else
        throw new ArgumentException($"Only DateTime, DateTimeOffset are supported.", nameof(propertyType));
#endif
    }
}
