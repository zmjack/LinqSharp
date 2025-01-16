﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

// <auto-generated/>
using NStandard;
using System.Reflection;

namespace LinqSharp;

internal static partial class MethodAccessor
{
    internal class Object
    {
        internal static MethodInfo ToStringMethod => lazy_ToStringMethod.Value;
        /// <summary>
        /// Method: System.String ToString()
        /// </summary>
        private static readonly Lazy<MethodInfo> lazy_ToStringMethod = new(() =>
        {
            return typeof(object)
                .GetMethodViaQualifiedName(
                    "System.String ToString()",
                    BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance
                );
        });

    }
}

