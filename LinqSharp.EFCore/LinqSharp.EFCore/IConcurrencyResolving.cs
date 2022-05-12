using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSharp.EFCore
{
    public interface IConcurrencyResolving
    {
        public int MaxConcurrencyRetry { get; }
    }
}
