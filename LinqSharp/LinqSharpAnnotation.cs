using System;

namespace LinqSharp
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
