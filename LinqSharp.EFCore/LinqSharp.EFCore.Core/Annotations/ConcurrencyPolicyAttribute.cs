﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp.EFCore.Annotations;


[AttributeUsage(AttributeTargets.Property)]
public sealed class ConcurrencyPolicyAttribute(ConcurrencyResolvingMode mode) : Attribute
{
    public ConcurrencyResolvingMode Mode { get; set; } = mode;
}
