using Microsoft.EntityFrameworkCore;
using System;

namespace NLinq.ProviderFunctions
{
    public static class PJet
    {
        [DbFunction]
        public static double Rnd() => throw new NotSupportedException();

    }
}
