using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSharp
{
    public class ConcurrencyPolicyAttribute : Attribute
    {
        public ConflictWin ConflictWin { get; set; }

        public ConcurrencyPolicyAttribute(ConflictWin conflictWin)
        {
            ConflictWin = conflictWin;
        }

    }
}
