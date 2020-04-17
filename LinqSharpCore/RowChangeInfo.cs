using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqSharp
{
    public class RowChangeInfo : Dictionary<string, FieldChangeInfo>
    {
        public RowChangeInfo(IEnumerable<PropertyEntry> entries)
        {
            foreach (var entry in entries)
            {
                Add(entry.Metadata.PropertyInfo.Name, new FieldChangeInfo
                {
                    Display = DataAnnotationEx.GetDisplayName(entry.Metadata.PropertyInfo),
                    Origin = entry.OriginalValue,
                    Current = entry.CurrentValue,
                });
            }
        }

    }
}
