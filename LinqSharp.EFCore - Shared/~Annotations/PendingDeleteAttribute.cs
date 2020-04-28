using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSharp
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    class PendingDeleteAttribute : Attribute
    {
    }
}
