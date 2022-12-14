using System;

namespace LinqSharp.EFCore
{
    public class NowTag : IAutoTag
    {
        public DateTime Now { get; set; }
        public DateTimeOffset NowOffset { get; set; }
    }
}
