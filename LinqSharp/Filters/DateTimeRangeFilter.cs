// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

#if NET6_0_OR_GREATER
using LinqSharp;
using LinqSharp.Filters;
using LinqSharp.Numeric;
using LinqSharp.Query;
using NStandard;
using System;

namespace LinqSharp.Filters;

public partial class DateTimeRangeFilter : IFieldFilter<DateTime>, IFieldFilter<DateTime?>
{
    public DateTimeType Type { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }

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

    public QueryExpression<DateTime?> Filter(QueryHelper<DateTime?> h)
    {
        DateTime? start, end;
        switch (Type)
        {
            case DateTimeType.Year:
                start = Start?.StartOfYear();
                end = End?.EndOfYear();
                break;

            case DateTimeType.Month:
                start = Start?.StartOfMonth();
                end = End?.EndOfMonth();
                break;

            case DateTimeType.Day:
                start = Start?.StartOfDay();
                end = End?.EndOfDay();
                break;

            case DateTimeType.Hour:
                start = Start?.StartOfHour();
                end = End?.EndOfHour();
                break;

            case DateTimeType.Minute:
                start = Start?.StartOfMinute();
                end = End?.EndOfMinute();
                break;

            default:
                start = Start?.StartOfSecond();
                end = End?.EndOfSecond();
                break;
        }

        var exp = h.Empty;
        if (start.HasValue) exp &= h.Where(x => start.Value <= x);
        if (end.HasValue) exp &= h.Where(x => x <= end.Value);

        return HasNull
            ? exp | h.Where(x => !x.HasValue)
            : exp & h.Where(x => x.HasValue);
    }

    public QueryExpression<DateTime> Filter(QueryHelper<DateTime> h)
    {
        DateTime? start, end;
        switch (Type)
        {
            case DateTimeType.Year:
                start = Start?.StartOfYear();
                end = End?.EndOfYear();
                break;

            case DateTimeType.Month:
                start = Start?.StartOfMonth();
                end = End?.EndOfMonth();
                break;

            case DateTimeType.Day:
                start = Start?.StartOfDay();
                end = End?.EndOfDay();
                break;

            case DateTimeType.Hour:
                start = Start?.StartOfHour();
                end = End?.EndOfHour();
                break;

            case DateTimeType.Minute:
                start = Start?.StartOfMinute();
                end = End?.EndOfMinute();
                break;

            default:
                start = Start?.StartOfSecond();
                end = End?.EndOfSecond();
                break;
        }

        var exp = h.Empty;
        if (start.HasValue) exp &= h.Where(x => start.Value <= x);
        if (end.HasValue) exp &= h.Where(x => x <= end.Value);

        return exp;
    }
}
#endif
