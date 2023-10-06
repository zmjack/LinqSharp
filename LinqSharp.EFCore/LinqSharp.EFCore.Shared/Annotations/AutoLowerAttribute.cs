// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using System;

namespace LinqSharp.EFCore.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public class AutoLowerAttribute : AutoAttribute
{
    public AutoLowerAttribute() : base(EntityState.Added, EntityState.Modified) { }
    public override object Format(object entity, Type propertyType, object value)
    {
        if (propertyType != typeof(string)) throw Exception_NotSupportedTypes(propertyType, nameof(propertyType));

        return (value as string)?.ToLower();
    }
}
