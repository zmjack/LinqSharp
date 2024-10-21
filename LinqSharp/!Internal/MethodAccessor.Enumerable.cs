﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

// <auto-generated/>
using NStandard;
using System;
using System.Reflection;

namespace LinqSharp;

internal static partial class MethodAccessor
{
    internal class Enumerable
    {
        internal static MethodInfo Any1 => lazy_Any1.Value;
        /// <summary>
        /// Method: Boolean Any[TSource](System.Collections.Generic.IEnumerable`1[TSource], System.Func`2[TSource,System.Boolean])
        /// </summary>
        private static readonly Lazy<MethodInfo> lazy_Any1 = new(() =>
        {
            return typeof(System.Linq.Enumerable)
                .GetMethodViaQualifiedName(
                    "Boolean Any[TSource](System.Collections.Generic.IEnumerable`1[TSource], System.Func`2[TSource,System.Boolean])",
                    BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static
                );
        });

        internal static MethodInfo OfType1 => lazy_OfType1.Value;
        /// <summary>
        /// Method: System.Collections.Generic.IEnumerable`1[TResult] OfType[TResult](System.Collections.IEnumerable)
        /// </summary>
        private static readonly Lazy<MethodInfo> lazy_OfType1 = new(() =>
        {
            return typeof(System.Linq.Enumerable)
                .GetMethodViaQualifiedName(
                    "System.Collections.Generic.IEnumerable`1[TResult] OfType[TResult](System.Collections.IEnumerable)",
                    BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static
                );
        });

        internal static MethodInfo Contains1 => lazy_Contains1.Value;
        /// <summary>
        /// Method: Boolean Contains[TSource](System.Collections.Generic.IEnumerable`1[TSource], TSource)
        /// </summary>
        private static readonly Lazy<MethodInfo> lazy_Contains1 = new(() =>
        {
            return typeof(System.Linq.Enumerable)
                .GetMethodViaQualifiedName(
                    "Boolean Contains[TSource](System.Collections.Generic.IEnumerable`1[TSource], TSource)",
                    BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static
                );
        });

        internal static MethodInfo Select1 => lazy_Select1.Value;
        /// <summary>
        /// Method: System.Collections.Generic.IEnumerable`1[TResult] Select[TSource,TResult](System.Collections.Generic.IEnumerable`1[TSource], System.Func`2[TSource,TResult])
        /// </summary>
        private static readonly Lazy<MethodInfo> lazy_Select1 = new(() =>
        {
            return typeof(System.Linq.Enumerable)
                .GetMethodViaQualifiedName(
                    "System.Collections.Generic.IEnumerable`1[TResult] Select[TSource,TResult](System.Collections.Generic.IEnumerable`1[TSource], System.Func`2[TSource,TResult])",
                    BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static
                );
        });

    }
}
