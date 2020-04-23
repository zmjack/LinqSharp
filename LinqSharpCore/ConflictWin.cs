using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSharp
{
    public enum ConflictWin
    {
        Unset,

        /// <summary>
        /// Client Win.
        /// </summary>
        Client,

        /// <summary>
        /// Store Win.
        /// </summary>
        Store,

        /// <summary>
        /// If orginal values are different from stored values, Store Win, else Client Win.
        /// </summary>
        Combine,
    }
}
