using System;

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
