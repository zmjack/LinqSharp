// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Linq;

namespace LinqSharp.EFCore
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoCondensedAttribute : AutoAttribute
    {
        public bool ReserveNewLine { get; set; }
        public bool Nullable { get; set; }

        public AutoCondensedAttribute() : base(EntityState.Added, EntityState.Modified)
        {
        }

        public override object Format(object value)
        {
            if (value is null) return Nullable ? null : string.Empty;
            if (value is not string) throw new ArgumentException("The value must be string.");

            if (ReserveNewLine)
            {
                //TODO: Optimizable
                var normalized = (value as string).NormalizeNewLine();
                var parts = from part in normalized.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) select part.Unique();
                return parts.Join(Environment.NewLine);
            }
            else return (value as string).Unique();
        }
    }
}
