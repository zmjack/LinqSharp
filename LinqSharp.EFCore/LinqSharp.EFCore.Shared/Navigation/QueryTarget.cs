// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace LinqSharp.EFCore.Navigation
{
    public struct QueryTarget
    {
        public Type PreviousProperty { get; set; }
        public Type PreviousPropertyElement { get; set; }
        public Type Property { get; set; }
        public LambdaExpression Expression { get; set; }
    }
}
