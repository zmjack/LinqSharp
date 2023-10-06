// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Query;

namespace LinqSharp;

public interface IFieldFilter<T>
{
    QueryExpression<T> Filter(QueryHelper<T> h);
}
