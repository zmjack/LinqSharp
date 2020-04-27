using Microsoft.EntityFrameworkCore;
using System;

namespace LinqSharp.ProviderFunctions
{
    public static class PJet
    {
        [DbFunction("RND")]
        public static double Rnd() => throw new NotSupportedException();

    }
}
