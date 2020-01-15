using System;
using System.ComponentModel;
using System.Linq;

namespace LinqSharp
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class XEnum
    {
        public static string DisplayName(this Enum @this)
        {
            var field = @this.GetType().GetFields().First(x => x.Name == @this.ToString());
            return DataAnnotationEx.GetDisplayName(field);
        }

    }
}
