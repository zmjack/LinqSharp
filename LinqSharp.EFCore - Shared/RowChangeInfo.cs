using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

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
                    IsModified = entry.IsModified,
                    Origin = entry.OriginalValue,
                    Current = entry.CurrentValue,
                });
            }
        }

    }
}
