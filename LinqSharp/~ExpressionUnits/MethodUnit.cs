// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System.Reflection;

namespace LinqSharp
{
    public static class MethodUnit
    {
        public static MethodInfo StringEquals => typeof(string).GetMethodViaQualifiedName("Boolean Equals(System.String)");
        public static MethodInfo StringContains => typeof(string).GetMethodViaQualifiedName("Boolean Contains(System.String)");
    }
}
