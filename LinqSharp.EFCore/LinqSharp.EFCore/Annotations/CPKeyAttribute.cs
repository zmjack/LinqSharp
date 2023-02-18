// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;

namespace LinqSharp.EFCore.Annotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CPKeyAttribute : Attribute
    {
        public int Order { get; set; }
        public CPKeyAttribute(int order) { Order = order; }
    }

}
