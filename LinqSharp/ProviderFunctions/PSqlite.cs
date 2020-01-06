using Microsoft.EntityFrameworkCore;
using System;

namespace LinqSharp.ProviderFunctions
{
    public static class PSqlite
    {
        [DbFunction("RANDOM")]
        public static double Random() => throw new NotSupportedException();

    }
}
