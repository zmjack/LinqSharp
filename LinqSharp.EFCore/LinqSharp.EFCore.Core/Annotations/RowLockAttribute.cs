// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;

namespace LinqSharp.EFCore.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public sealed class RowLockAttribute : Attribute
{
    public string[]? Columns { get; }
    public int Order { get; set; }

    public RowLockAttribute(string[]? columns = null, int order = 0)
    {
        Columns = columns;
        Order = order;
    }
}
