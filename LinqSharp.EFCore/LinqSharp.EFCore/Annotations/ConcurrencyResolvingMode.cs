// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;

namespace LinqSharp.EFCore.Annotations
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
        /// [Warning] Do not specify manually. If property is one of <see cref="ConcurrencyCheckAttribute" /> or <see cref="TimestampAttribute", it will be set automatically.
        /// </summary>
        Check = 0x100,
    }
}
