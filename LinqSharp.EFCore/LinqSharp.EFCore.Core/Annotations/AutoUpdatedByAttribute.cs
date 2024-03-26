// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Annotations.Params;
using System;

namespace LinqSharp.EFCore.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public sealed class AutoUpdatedByAttribute : SpecialAutoAttribute<UserParam>
{
    public AutoUpdatedByAttribute() : base(AutoState.Added, AutoState.Modified) { }

    public override object Format(object entity, Type propertyType, UserParam value)
    {
        return value.CurrentUser;
    }
}
