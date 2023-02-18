// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Design.AutoTags;
using Microsoft.EntityFrameworkCore;
using System;

namespace LinqSharp.EFCore.Annotations.Base
{
    public abstract class SpecialAutoAttribute<TAutoTag> : AutoAttribute where TAutoTag : IAutoTag
    {
        public SpecialAutoAttribute() : base() { }
        public SpecialAutoAttribute(params EntityState[] states) : base(states) { }

        public override object Format(object entity, Type propertyType, object value)
        {
            if (value is not TAutoTag autoTag) throw new ArgumentException($"Only {typeof(TAutoTag)} is supported.", nameof(value));

            return Format(entity, propertyType, autoTag);
        }
        public abstract object Format(object entity, Type propertyType, TAutoTag value);
    }
}
