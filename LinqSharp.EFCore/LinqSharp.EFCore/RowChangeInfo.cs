﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp.EFCore
{
    public class RowChangeInfo : Dictionary<string, FieldChangeInfo>
    {
        public bool IsValid { get; set; }

        public RowChangeInfo() { }
        public RowChangeInfo(IEnumerable<PropertyEntry> entries) : this(entries, false) { }
        public RowChangeInfo(IEnumerable<PropertyEntry> entries, bool modifiedOnly)
        {
            if (modifiedOnly)
            {
                entries = entries.Where(x => x.IsModified).Where(x => (x.OriginalValue == null && x.CurrentValue == null) || (x.OriginalValue?.Equals(x.CurrentValue) ?? false));
            }

            if (entries.Any()) IsValid = true;

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
