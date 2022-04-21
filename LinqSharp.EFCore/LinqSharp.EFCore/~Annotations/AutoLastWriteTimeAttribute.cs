// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using System;

namespace LinqSharp.EFCore
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoLastWriteTimeAttribute : AutoAttribute
    {
        public AutoLastWriteTimeAttribute() : base(EntityState.Added, EntityState.Modified) { }
        public override object Format(object value) => throw new NotImplementedException();
    }
}
