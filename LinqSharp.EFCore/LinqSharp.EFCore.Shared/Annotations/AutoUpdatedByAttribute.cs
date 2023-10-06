// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Design.AutoTags;
using Microsoft.EntityFrameworkCore;
using System;

namespace LinqSharp.EFCore.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public class AutoUpdatedByAttribute : SpecialAutoAttribute<UserTag>
{
    public AutoUpdatedByAttribute() : base(EntityState.Added, EntityState.Modified) { }

    public override object Format(object entity, Type propertyType, UserTag value)
    {
        return value.CurrentUser;
    }
}
