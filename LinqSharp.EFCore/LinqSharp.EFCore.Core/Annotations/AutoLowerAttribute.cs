// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp.EFCore.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public sealed class AutoLowerAttribute : AutoAttribute
{
    public AutoLowerAttribute() : base(AutoState.Added, AutoState.Modified) { }
    public override object? Format(object entity, Type propertyType, object? value)
    {
        if (propertyType != typeof(string)) throw Exception_NotSupportedTypes(propertyType, nameof(propertyType));

        return (value as string)?.ToLower();
    }
}
