using Microsoft.EntityFrameworkCore;
using System;

namespace NLinq.ProviderFunctions
{
    public static class PSqlServer
    {
        [DbFunction]
        public static double Rand() => throw new NotSupportedException();

    }
}
