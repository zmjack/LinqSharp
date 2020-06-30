using Microsoft.EntityFrameworkCore;
using System;

#pragma warning disable IDE0060 // Remove unused parameter

namespace LinqSharp.EFCore.ProviderFunctions
{
    public static class PJet
    {
        [DbFunction("RND")]
        public static double Rnd() => throw new NotSupportedException();
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
