// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp.EFCore.Entities;

public class FieldChangeInfo
{
    public string? Display { get; set; }
    public bool IsModified { get; set; }
    public object? Origin { get; set; }
    public object? Current { get; set; }
}
