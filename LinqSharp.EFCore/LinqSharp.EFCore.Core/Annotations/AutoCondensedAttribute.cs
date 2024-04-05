// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;
using System.Linq;

namespace LinqSharp.EFCore.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public sealed class AutoCondensedAttribute : AutoAttribute
{
    public bool ReserveNewLine { get; set; }
    public bool Nullable { get; set; }

    public AutoCondensedAttribute() : base(AutoState.Added, AutoState.Modified) { }

    public override object? Format(object entity, Type propertyType, object? value)
    {
        if (propertyType != typeof(string)) throw Exception_NotSupportedTypes(propertyType, nameof(propertyType));

        if (value is null) return Nullable ? null : string.Empty;

        var @string = value as string;
        if (ReserveNewLine)
        {
            //TODO: Optimizable
            var normalized = @string.NormalizeNewLine();
            var parts = from part in normalized.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) select part.Unique();
            return parts.Join(Environment.NewLine);
        }
        else return @string.Unique();
    }
}
