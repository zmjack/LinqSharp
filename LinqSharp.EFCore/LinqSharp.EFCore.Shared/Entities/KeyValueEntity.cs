// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.EFCore.Entities;

public abstract class KeyValueEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [IndexField(IndexType.Unique, Group = "Item&Key")]
    [StringLength(127)]
    public string Item { get; set; }

    [Required]
    [IndexField(IndexType.Unique, Group = "Item&Key")]
    [StringLength(127)]
    public string Key { get; set; }

    [StringLength(768)]
    public string Value { get; set; }
}
