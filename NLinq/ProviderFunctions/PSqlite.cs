using Microsoft.EntityFrameworkCore;
using System;

namespace NLinq.ProviderFunctions
{
    public static class PSqlite
    {
        [DbFunction("RANDOM")]
        public static double Random() => throw new NotSupportedException();

    }
}
