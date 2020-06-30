using Microsoft.EntityFrameworkCore;
using System;

#pragma warning disable IDE0060 // Remove unused parameter

namespace LinqSharp.EFCore.ProviderFunctions
{
    public static class POracle
    {
        [DbFunction("RANDOM", "DBMS_RANDOM")]
        public static double Random() => throw new NotSupportedException();
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
