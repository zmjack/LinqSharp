// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Annotations.Base;
using LinqSharp.EFCore.Design.AutoTags;
using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Linq;

namespace LinqSharp.EFCore.Annotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoCreationTimeAttribute : SpecialAutoAttribute<NowTag>
    {
        private static readonly Type[] DateTimeTypes = new Type[] { typeof(DateTime), typeof(DateTime?) };
        private static readonly Type[] DateTimeOffsetTypes = new Type[] { typeof(DateTimeOffset), typeof(DateTimeOffset?) };

        public AutoCreationTimeAttribute() : base(EntityState.Added) { }

        public override object Format(object entity, Type propertyType, NowTag value)
        {
            if (DateTimeTypes.Contains(propertyType)) return value.Now;
            else if (DateTimeOffsetTypes.Contains(propertyType)) return value.NowOffset;
            else throw new ArgumentException($"Only {DateTimeTypes.Join(", ")}, {DateTimeOffsetTypes.Join(", ")} are supported.", nameof(propertyType));
        }
    }
}
