// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Caching.Memory;
using NStandard;
using System;
using System.Linq;
using System.Reflection;

namespace LinqSharp.Query
{
    public static class MethodContainer
    {
        private static readonly MemoryCache GenericCache = new(new MemoryCacheOptions());
        private static readonly MemoryCache Cache = new(new MemoryCacheOptions());

        public static MethodInfo GenericOfType => GenericCache.GetOrCreate($"${nameof(Enumerable)}.{nameof(Enumerable.OfType)}", entry =>
        {
            var generic = typeof(Enumerable).GetMethodViaQualifiedName("System.Collections.Generic.IEnumerable`1[TResult] OfType[TResult](System.Collections.IEnumerable)");
            return generic;
        });
        public static MethodInfo GenericContains => GenericCache.GetOrCreate($"${nameof(Enumerable)}.{nameof(Enumerable.Contains)}", entry =>
        {
            var generic = typeof(Enumerable).GetMethodViaQualifiedName("Boolean Contains[TSource](System.Collections.Generic.IEnumerable`1[TSource], TSource)");
            return generic;
        });

        public static MethodInfo StringConcat => Cache.GetOrCreate(nameof(StringConcat), entry => typeof(string).GetMethodViaQualifiedName("System.String Concat(System.String, System.String)"));
        public static MethodInfo StringEquals => Cache.GetOrCreate(nameof(StringEquals), entry => typeof(string).GetMethodViaQualifiedName("Boolean Equals(System.String)"));
        public static MethodInfo StringContains => Cache.GetOrCreate(nameof(StringContains), entry => typeof(string).GetMethodViaQualifiedName("Boolean Contains(System.String)"));

        public static MethodInfo Int16Equals => Cache.GetOrCreate(nameof(Int16Equals), entry => typeof(short).GetMethodViaQualifiedName("Boolean Equals(System.Int16)"));
        public static MethodInfo UInt16Equals => Cache.GetOrCreate(nameof(UInt16Equals), entry => typeof(ushort).GetMethodViaQualifiedName("Boolean Equals(System.UInt16)"));
        public static MethodInfo Int32Equals => Cache.GetOrCreate(nameof(Int32Equals), entry => typeof(int).GetMethodViaQualifiedName("Boolean Equals(System.Int32)"));
        public static MethodInfo UInt32Equals => Cache.GetOrCreate(nameof(UInt32Equals), entry => typeof(uint).GetMethodViaQualifiedName("Boolean Equals(System.UInt32)"));
        public static MethodInfo Int64Equals => Cache.GetOrCreate(nameof(Int64Equals), entry => typeof(long).GetMethodViaQualifiedName("Boolean Equals(System.Int64)"));
        public static MethodInfo UInt64Equals => Cache.GetOrCreate(nameof(UInt64Equals), entry => typeof(ulong).GetMethodViaQualifiedName("Boolean Equals(System.UInt64)"));
        public static MethodInfo SingleEquals => Cache.GetOrCreate(nameof(SingleEquals), entry => typeof(float).GetMethodViaQualifiedName("Boolean Equals(System.Single)"));
        public static MethodInfo DoubleEquals => Cache.GetOrCreate(nameof(DoubleEquals), entry => typeof(double).GetMethodViaQualifiedName("Boolean Equals(System.Double)"));
        public static MethodInfo DateTimeEquals => Cache.GetOrCreate(nameof(DateTimeEquals), entry => typeof(DateTime).GetMethodViaQualifiedName("Boolean Equals(System.DateTime)"));
        public static MethodInfo GuidEquals => Cache.GetOrCreate(nameof(GuidEquals), entry => typeof(DateTime).GetMethodViaQualifiedName("Boolean Equals(System.Guid)"));

        public static MethodInfo NullableInt16Equals => Cache.GetOrCreate(nameof(NullableInt16Equals), entry => typeof(short?).GetMethodViaQualifiedName("Boolean Equals(System.Object)"));
        public static MethodInfo NullableUInt16Equals => Cache.GetOrCreate(nameof(NullableUInt16Equals), entry => typeof(ushort?).GetMethodViaQualifiedName("Boolean Equals(System.Object)"));
        public static MethodInfo NullableInt32Equals => Cache.GetOrCreate(nameof(NullableInt32Equals), entry => typeof(int?).GetMethodViaQualifiedName("Boolean Equals(System.Object)"));
        public static MethodInfo NullableUInt32Equals => Cache.GetOrCreate(nameof(NullableUInt32Equals), entry => typeof(uint?).GetMethodViaQualifiedName("Boolean Equals(System.Object)"));
        public static MethodInfo NullableInt64Equals => Cache.GetOrCreate(nameof(NullableInt64Equals), entry => typeof(long?).GetMethodViaQualifiedName("Boolean Equals(System.Object)"));
        public static MethodInfo NullableUInt64Equals => Cache.GetOrCreate(nameof(NullableUInt64Equals), entry => typeof(ulong?).GetMethodViaQualifiedName("Boolean Equals(System.Object)"));
        public static MethodInfo NullableSingleEquals => Cache.GetOrCreate(nameof(NullableSingleEquals), entry => typeof(float?).GetMethodViaQualifiedName("Boolean Equals(System.Object)"));
        public static MethodInfo NullableDoubleEquals => Cache.GetOrCreate(nameof(NullableDoubleEquals), entry => typeof(double?).GetMethodViaQualifiedName("Boolean Equals(System.Object)"));
        public static MethodInfo NullableDateTimeEquals => Cache.GetOrCreate(nameof(NullableDateTimeEquals), entry => typeof(DateTime?).GetMethodViaQualifiedName("Boolean Equals(System.Object)"));
        public static MethodInfo NullableGuidEquals => Cache.GetOrCreate(nameof(NullableGuidEquals), entry => typeof(DateTime?).GetMethodViaQualifiedName("Boolean Equals(System.Object)"));

    }
}
