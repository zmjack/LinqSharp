// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System.Linq;

namespace LinqSharp;

public interface IQueryFilter<TSource>
{
    IQueryable<TSource> Apply(IQueryable<TSource> source);
}
