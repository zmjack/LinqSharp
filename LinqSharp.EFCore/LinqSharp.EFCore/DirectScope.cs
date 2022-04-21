using NStandard;
using System;

namespace LinqSharp.EFCore
{
    public class DirectScope : Scope<DirectScope>
    {
        public static Exception RunningOutsideScopeException = new InvalidOperationException($"Direct action is running outside {nameof(DirectScope)}.");
    }
}
