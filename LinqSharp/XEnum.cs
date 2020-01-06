using System;
using System.Linq;

namespace LinqSharp
{
    public static class XEnum
    {
        public static string DisplayName(this Enum @this)
        {
            var field = @this.GetType().GetFields().First(x => x.Name == @this.ToString());
            return DataAnnotationUtility.GetDisplayName(field);
        }

    }
}
