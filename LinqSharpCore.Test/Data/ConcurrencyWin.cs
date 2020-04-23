using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSharp.Data.Test
{
    public class ConcurrencyWin : Attribute
    {
        public ConflictWin ConflictWin { get; set; }

        public ConcurrencyWin(ConflictWin conflictWin)
        {
            ConflictWin = conflictWin;
        }
    }
}
