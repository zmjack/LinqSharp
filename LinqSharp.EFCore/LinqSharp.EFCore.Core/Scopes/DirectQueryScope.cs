// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;

namespace LinqSharp.EFCore.Scopes;

public class DirectQueryScope : Scope<DirectQueryScope>
{
    public static InvalidOperationException RunningOutsideScopeException => new($"Direct action is running outside {nameof(DirectQueryScope)}.");

    public DirectQueryScope()
    {
    }
}
