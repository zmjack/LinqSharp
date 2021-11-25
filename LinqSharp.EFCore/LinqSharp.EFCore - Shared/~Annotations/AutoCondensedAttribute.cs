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

        public AutoCondensedAttribute(bool reserveNewLine = false) : base(EntityState.Added, EntityState.Modified)
        {
            ReserveNewLine = reserveNewLine;
        }

        public override object Format(object value)
        {
            if (value is null) return "";
            if (value is not string) throw new ArgumentException("The value must be string.");
            else
            {
                var str = value as string;
                if (ReserveNewLine)
                {
                    //TODO: Optimizable
                    var normalized = str.NormalizeNewLine();
                    return (from part in normalized.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) select part.Unique()).Join(Environment.NewLine);
                }
                else return str.Unique();
            }
        }
    }
}
