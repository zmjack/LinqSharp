// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;
using System.Reflection;

namespace LinqSharp
{
    public static class MethodUnit
    {
        public static MethodInfo StringEquals => typeof(string).GetMethodViaQualifiedName("Boolean Equals(System.String)");
        public static MethodInfo StringContains => typeof(string).GetMethodViaQualifiedName("Boolean Contains(System.String)");

        public static MethodInfo Int16Equals => typeof(short).GetMethodViaQualifiedName("Boolean Equals(System.Int16)");
        public static MethodInfo UInt16Equals => typeof(ushort).GetMethodViaQualifiedName("Boolean Equals(System.UInt16)");
        public static MethodInfo Int32Equals => typeof(int).GetMethodViaQualifiedName("Boolean Equals(System.Int32)");
        public static MethodInfo UInt32Equals => typeof(uint).GetMethodViaQualifiedName("Boolean Equals(System.UInt32)");
        public static MethodInfo Int64Equals => typeof(long).GetMethodViaQualifiedName("Boolean Equals(System.Int64)");
        public static MethodInfo UInt64Equals => typeof(ulong).GetMethodViaQualifiedName("Boolean Equals(System.UInt64)");
        public static MethodInfo SingleEquals => typeof(float).GetMethodViaQualifiedName("Boolean Equals(System.Single)");
        public static MethodInfo DoubleEquals => typeof(double).GetMethodViaQualifiedName("Boolean Equals(System.Double)");
        public static MethodInfo DateTimeEquals => typeof(DateTime).GetMethodViaQualifiedName("Boolean Equals(System.DateTime)");
        public static MethodInfo GuidEquals => typeof(DateTime).GetMethodViaQualifiedName("Boolean Equals(System.Guid)");

        public static MethodInfo NullableInt16Equals => typeof(short?).GetMethodViaQualifiedName("Boolean Equals(System.Object)");
        public static MethodInfo NullableUInt16Equals => typeof(ushort?).GetMethodViaQualifiedName("Boolean Equals(System.Object)");
        public static MethodInfo NullableInt32Equals => typeof(int?).GetMethodViaQualifiedName("Boolean Equals(System.Object)");
        public static MethodInfo NullableUInt32Equals => typeof(uint?).GetMethodViaQualifiedName("Boolean Equals(System.Object)");
        public static MethodInfo NullableInt64Equals => typeof(long?).GetMethodViaQualifiedName("Boolean Equals(System.Object)");
        public static MethodInfo NullableUInt64Equals => typeof(ulong?).GetMethodViaQualifiedName("Boolean Equals(System.Object)");
        public static MethodInfo NullableSingleEquals => typeof(float?).GetMethodViaQualifiedName("Boolean Equals(System.Object)");
        public static MethodInfo NullableDoubleEquals => typeof(double?).GetMethodViaQualifiedName("Boolean Equals(System.Object)");
        public static MethodInfo NullableDateTimeEquals => typeof(DateTime?).GetMethodViaQualifiedName("Boolean Equals(System.Object)");
        public static MethodInfo NullableGuidEquals => typeof(DateTime?).GetMethodViaQualifiedName("Boolean Equals(System.Object)");

    }
}
