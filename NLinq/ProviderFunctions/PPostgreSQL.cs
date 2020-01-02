using Microsoft.EntityFrameworkCore;
using System;

namespace NLinq.ProviderFunctions
{
    public static class PPostgreSQL
    {
        [DbFunction("RANDOM")]
        public static double Random() => throw new NotSupportedException();

    }
}
