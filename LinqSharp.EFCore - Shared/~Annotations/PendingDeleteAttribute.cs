using System;

namespace LinqSharp.EFCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    class PendingDeleteAttribute : Attribute
    {
    }
}
