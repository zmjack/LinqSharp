// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq;

namespace LinqSharp
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EnumExtensions
    {
        public static string DisplayName(this Enum @this)
        {
            var field = @this.GetType().GetFields().First(x => x.Name == @this.ToString());
            return DataAnnotation.GetDisplayName(field);
        }

    }
}
