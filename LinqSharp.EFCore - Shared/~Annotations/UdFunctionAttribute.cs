// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;

namespace LinqSharp.EFCore
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class UdFunctionAttribute : Attribute
    {
        public DatabaseProviderName ProviderName { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }

        public UdFunctionAttribute(DatabaseProviderName providerName, string name, string schema = null)
        {
            ProviderName = providerName;
            Name = name;
            Schema = schema;
        }
    }

}
