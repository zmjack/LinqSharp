// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp.EFCore
{
    public enum ConflictWin
    {
        Throw,

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
