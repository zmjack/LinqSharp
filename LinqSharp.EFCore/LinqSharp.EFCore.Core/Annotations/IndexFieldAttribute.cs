﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp.EFCore.Annotations;

public enum IndexType { Normal, Unique }

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class IndexFieldAttribute(IndexType type) : Attribute
{
    public string? Group { get; set; }
    public IndexType Type { get; set; } = type;
}
