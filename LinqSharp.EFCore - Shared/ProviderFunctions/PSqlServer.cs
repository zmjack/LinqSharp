using Microsoft.EntityFrameworkCore;
using System;

#pragma warning disable IDE0060 // Remove unused parameter

namespace LinqSharp.EFCore.ProviderFunctions
{
    public static class PSqlServer
    {
        [DbFunction("RAND")]
        public static double Rand() => throw new NotSupportedException();
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
