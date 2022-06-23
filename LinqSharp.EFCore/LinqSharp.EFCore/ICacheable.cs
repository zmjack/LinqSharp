﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp.EFCore
{
    public interface ICacheable<TDataSource> where TDataSource : class, new()
    {
        public TDataSource Source { get; }
        public void OnCache();
    }

}