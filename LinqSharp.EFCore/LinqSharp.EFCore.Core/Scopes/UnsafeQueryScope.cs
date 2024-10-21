﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Annotations;
using NStandard;

namespace LinqSharp.EFCore.Scopes;

public class UnsafeQueryScope : Scope<UnsafeQueryScope>
{
    public class Throws
    {
        public static InvalidOperationException RunningOutsideScopeException() => new($"Unsafe operations cannot be run outside of {nameof(UnsafeQueryScope)}.");
    }


    public UnsafeQueryScope()
    {
    }
}