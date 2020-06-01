using Microsoft.EntityFrameworkCore;
using System;

namespace LinqSharp.EFCore.ProviderFunctions
{
    public static class PMySql
    {
        [DbFunction("RAND")]
        public static double Rand() => throw new NotSupportedException();

    }
}
