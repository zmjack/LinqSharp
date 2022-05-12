// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp.EFCore
{
    public enum ConcurrencyResolvingMode
    {
        /// <summary>
        /// Client Win. (Default)
        /// </summary>
        ClientWins = 0,

        /// <summary>
        /// Database Win.
        /// </summary>
        DatabaseWins = 1,

        /// <summary>
        /// Store Win.
        /// </summary>
        Check = 0x100,
    }
}
