using System;

namespace LinqSharp.EFCore
{
    [Flags]
    public enum LinqSharpAnnotation
    {
        Index = 1,
        Provider = 2,
        CompositeKey = 4,
        All = Index | Provider | CompositeKey,
    }
}
