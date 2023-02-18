using System;

namespace LinqSharp.EFCore.Annotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConcurrencyResolvableAttribute : Attribute
    {
    }
}
