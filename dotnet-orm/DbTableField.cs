// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;

namespace LinqSharp.Cli
{
    public class DbTableField
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Type RuntimeType { get; set; }
        public int? MaxLength { get; set; }
        public string Index { get; set; }
        public bool Required { get; set; }
        public Type ReferenceType { get; set; }

    }
}
