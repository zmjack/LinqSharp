// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using NStandard.UnitValues;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LinqSharp
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class XIUnitValue
    {
        private static T Default<T>() where T : struct, IUnitValue
        {
            var obj = new T();
            var unit = obj.GetDefaultUnit();
            obj.WithUnit(unit);
            return obj;
        }

        public static T QSum<T>(this IEnumerable<T> source) where T : struct, IUnitValue
        {
            if (!source.Any()) return Default<T>();

            var value = Enumerable.Sum(source, x => x.OriginalValue);
            var unit = source.First().Unit;
            return new T { OriginalValue = value, Unit = unit };
        }
        public static T? QSum<T>(this IEnumerable<T?> source) where T : struct, IUnitValue
        {
            if (!source.Any()) return Default<T>();
            var value = Enumerable.Sum(source, x => x?.OriginalValue);
            if (!value.HasValue) return Default<T>();

            var unit = source.FirstOrDefault(x => x is not null)?.Unit ?? Default<T>().Unit;
            return new T { OriginalValue = value.Value, Unit = unit };
        }

        public static T QAverage<T>(this IEnumerable<T> source) where T : struct, IUnitValue
        {
            if (!source.Any()) throw new InvalidOperationException("Sequence contains no elements");

            var value = Enumerable.Average(source, x => x.OriginalValue);
            var unit = source.First().Unit;
            return new T { OriginalValue = value, Unit = unit };
        }
        public static T? QAverage<T>(this IEnumerable<T?> source) where T : struct, IUnitValue
        {
            if (!source.Any()) return null;
            var value = Enumerable.Average(source, x => x?.OriginalValue);
            if (!value.HasValue) return null;

            var unit = source.FirstOrDefault(x => x is not null)?.Unit ?? Default<T>().Unit;
            return new T { OriginalValue = value.Value, Unit = unit };
        }
    }
}
