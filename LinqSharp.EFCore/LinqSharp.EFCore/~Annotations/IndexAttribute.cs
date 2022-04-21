// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;

namespace LinqSharp.EFCore
{
    public enum IndexType { Normal, Unique }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class IndexAttribute : Attribute
    {
        public string Group { get; set; }
        public IndexType Type { get; set; }
        public IndexAttribute(IndexType type)
        {
            Type = type;
        }
    }

}
