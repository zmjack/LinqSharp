// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LinqSharp.EFCore.Functions
{
    public interface IDbFuncProvider
    {
        void UseRandom();
        void UseConcat();
        void UseDateTime();
        void UseToDouble();
    }
}
