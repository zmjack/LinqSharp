// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;

namespace LinqSharp.EFCore.Query
{
    public class DirectScope : Scope<DirectScope>
    {
        public static Exception RunningOutsideScopeException = new InvalidOperationException($"Direct action is running outside {nameof(DirectScope)}.");

        internal DirectScope()
        {
        }

    }
}
