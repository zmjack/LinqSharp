// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Castle.DynamicProxy.Contributors;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

namespace LinqSharp.EFCore
{
    public class RowChangeInfo : Dictionary<string, FieldChangeInfo>
    {
        public RowChangeInfo() { }
        public RowChangeInfo(IEnumerable<PropertyEntry> entries)
        {
            foreach (var entry in entries)
            {
                Add(entry.Metadata.PropertyInfo.Name, new FieldChangeInfo
                {
                    Display = DataAnnotationEx.GetDisplayName(entry.Metadata.PropertyInfo),
                    IsModified = entry.IsModified,
                    Origin = entry.OriginalValue,
                    Current = entry.CurrentValue,
                });
            }
        }

    }
}
