// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System.Collections;

namespace LinqSharp.Query;

public interface IEnumerablePage : IEnumerable
{
    int PageNumber { get; }
    int PageSize { get; }
    int PageCount { get; }
    int SourceCount { get; }
    bool IsFristPage { get; }
    bool IsLastPage { get; }
}

public interface IEnumerablePage<T> : IEnumerablePage, IEnumerable<T>
{
    IEnumerable<T> Items { get; }
}
