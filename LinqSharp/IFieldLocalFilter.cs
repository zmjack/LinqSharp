using System;

namespace LinqSharp
{
    public interface IFieldLocalFilter<T>
    {
        Func<T, bool> Filter { get; }
    }
}
