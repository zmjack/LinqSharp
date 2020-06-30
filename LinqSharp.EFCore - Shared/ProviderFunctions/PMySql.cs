using Microsoft.EntityFrameworkCore;
using System;

#pragma warning disable IDE0060 // Remove unused parameter

namespace LinqSharp.EFCore.ProviderFunctions
{
    public static class PMySql
    {
        [DbFunction("RAND")]
        public static double Rand() => throw new NotSupportedException();

        [DbFunction("STR_TO_DATE")]
        public static DateTime StrToDate(string str, string format) => throw new NotSupportedException();
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
