using System;

namespace LinqSharp.EFCore.Data.Test
{
    public class CPKeyModel
    {
        [CPKey(1)]
        public Guid Id1 { get; set; }

        [CPKey(2)]
        public Guid Id2 { get; set; }

    }
}
