// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

#if NET6_0_OR_GREATER
using LinqSharp.Query;
using NStandard;
using System;
using System.Diagnostics;

namespace LinqSharp.Filters;

[DebuggerDisplay("{Type}, {(Start.HasValue ? Start.ToString() : \"<null>\")} to {(End.HasValue ? End.ToString() : \"<null>\")}")]
public class DateOnlyRangeFilter : IFieldFilter<DateOnly>, IFieldFilter<DateOnly?>
{
    public DateOnlyType Type { get; set; }
    public DateOnly? Start { get; set; }
    public DateOnly? End { get; set; }

    /// <summary>
    /// Indicates whether to include null values for the field.
    /// </summary>
    /// <remarks>
    /// Explanation:<br/>
    /// <code>
    /// ○ Valid only when using DateTime?.<br/>
    /// </code>
    /// </remarks>
    public bool HasNull { get; set; }

    public QueryExpression<DateOnly?> Filter(QueryHelper<DateOnly?> h)
    {
        if (!Start.HasValue && !End.HasValue) return h.Empty;

        DateOnly? start, end;
        switch (Type)
        {
            case DateOnlyType.Year:
                start = Start?.StartOfYear();
                end = End?.EndOfYear();
                break;

            case DateOnlyType.Month:
                start = Start?.StartOfMonth();
                end = End?.EndOfMonth();
                break;

            default:
                start = Start;
                end = End;
                break;
        }

        var exp = h.Empty;
        if (start.HasValue) exp &= h.Where(x => start.Value <= x);
        if (end.HasValue) exp &= h.Where(x => x <= end.Value);

        return HasNull
            ? exp | h.Where(x => !x.HasValue)
            : exp & h.Where(x => x.HasValue);
    }

    public QueryExpression<DateOnly> Filter(QueryHelper<DateOnly> h)
    {
        if (!Start.HasValue && !End.HasValue) return h.Empty;

        DateOnly? start, end;
        switch (Type)
        {
            case DateOnlyType.Year:
                start = Start?.StartOfYear();
                end = End?.EndOfYear();
                break;

            case DateOnlyType.Month:
                start = Start?.StartOfMonth();
                end = End?.EndOfMonth();
                break;

            default:
                start = Start;
                end = End;
                break;
        }

        var exp = h.Empty;
        if (start.HasValue) exp &= h.Where(x => start.Value <= x);
        if (end.HasValue) exp &= h.Where(x => x <= end.Value);

        return exp;
    }

}
#endif
