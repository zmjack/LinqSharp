using System;

namespace LinqSharp
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    class PendingDeleteAttribute : Attribute
    {
    }
}
