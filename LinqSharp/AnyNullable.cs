using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSharp
{
    public struct AnyNullable<T>
    {
        public bool HasValue { get; set; }
        public T Value { get; set; }
    }
}
