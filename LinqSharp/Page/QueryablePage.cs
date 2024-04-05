// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.Page;

public class QueryablePage<T> : IQueryablePage<T>
{
    public IEnumerable<T> Items { get; protected set; }
    public int PageNumber { get; protected set; }
    public int PageSize { get; protected set; }
    public int PageCount { get; protected set; }
    public int SourceCount { get; protected set; }
    public bool IsFristPage => PageNumber == 1;
    public bool IsLastPage => PageNumber == PageCount;

    public Type ElementType => (Items as IQueryable<T>)!.ElementType;
    public Expression Expression => (Items as IQueryable<T>)!.Expression;
    public IQueryProvider Provider => (Items as IQueryable<T>)!.Provider;

    public QueryablePage(IQueryable<T> source, int page, int pageSize)
    {
        if (page < 1) throw new ArgumentException("Page must be greater than 0.");
        if (pageSize < 1) throw new ArgumentException("PageSize must be greater than 0.");

        PageSize = pageSize;
        PageCount = source.PageCount(pageSize, out var sourceCount);
        SourceCount = sourceCount;
        PageNumber = page;
        Items = source.Skip((PageNumber - 1) * PageSize).Take(PageSize);
    }

    public EnumerablePage<T> ToEnumerable() => new(this);

    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
}
