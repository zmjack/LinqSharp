// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Utils;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LinqSharp.EFCore.Entities;

public class RowChangeInfo : Dictionary<string, FieldChangeInfo>
{
    public bool IsValid { get; set; }

    public RowChangeInfo() { }
    public RowChangeInfo(IEnumerable<PropertyEntry> entries) : this(entries, false) { }
    public RowChangeInfo(IEnumerable<PropertyEntry> entries, bool modifiedOnly)
    {
        if (modifiedOnly)
        {
            entries = from entry in entries
                      where entry.IsModified
                      where !(entry.OriginalValue is null && entry.CurrentValue is null) && !(entry.OriginalValue?.Equals(entry.CurrentValue) ?? false)
                      select entry;
        }

        if (entries.Any()) IsValid = true;

        foreach (var entry in entries)
        {
            Add(entry.Metadata.PropertyInfo!.Name, new FieldChangeInfo
            {
                Display = DataAnnotation.GetDisplayName(entry.Metadata.PropertyInfo),
                IsModified = entry.IsModified,
                Origin = entry.OriginalValue,
                Current = entry.CurrentValue,
            });
        }
    }

}
