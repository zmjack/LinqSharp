using Microsoft.EntityFrameworkCore;
using System;

#pragma warning disable IDE0060 // Remove unused parameter

namespace LinqSharp.EFCore.ProviderFunctions
{
    public static class PSqlite
    {
        [DbFunction("RANDOM")]
        public static double Random() => throw new NotSupportedException();

        [DbFunction("STRFTIME")]
        public static string Strftime(string format, string timestring) => throw new NotSupportedException();
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
