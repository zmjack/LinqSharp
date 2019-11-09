using Microsoft.EntityFrameworkCore;
using System;

namespace NLinq.ProviderFunctions
{
    public static class PMySql
    {
        [DbFunction]
        public static double Rand() => throw new NotSupportedException();

    }
}
