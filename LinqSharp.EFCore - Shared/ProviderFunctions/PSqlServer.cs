using Microsoft.EntityFrameworkCore;
using System;

namespace LinqSharp.ProviderFunctions
{
    public static class PSqlServer
    {
        [DbFunction("RAND")]
        public static double Rand() => throw new NotSupportedException();

    }
}
