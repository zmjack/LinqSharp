using System;

namespace LinqSharp.EFCore
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConcurrencyResolvableAttribute : Attribute
    {
    }
}
