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
            obj.Init();
            return obj;
        }

        public static T QSum<T>(this IEnumerable<T> source) where T : struct, IUnitValue
        {
            if (!source.Any()) return Default<T>();

            var clone = source.First();
            clone.OriginalValue = Enumerable.Sum(source, x => x.OriginalValue);
            return clone;
        }
        public static T? QSum<T>(this IEnumerable<T?> source) where T : struct, IUnitValue
        {
            if (!source.Any()) return Default<T>();
            var value = Enumerable.Sum(source, x => x?.OriginalValue);
            if (!value.HasValue) return Default<T>();

            var clone = source.FirstOrDefault(x => x.HasValue).Value;
            clone.OriginalValue = value.Value;
            return clone;
        }

        public static T QAverage<T>(this IEnumerable<T> source) where T : struct, IUnitValue
        {
            if (!source.Any()) throw new InvalidOperationException("Sequence contains no elements");

            var clone = source.First();
            clone.OriginalValue = Enumerable.Average(source, x => x.OriginalValue);
            return clone;
        }
        public static T? QAverage<T>(this IEnumerable<T?> source) where T : struct, IUnitValue
        {
            if (!source.Any()) return null;
            var value = Enumerable.Average(source, x => x?.OriginalValue);
            if (!value.HasValue) return null;

            var clone = source.FirstOrDefault(x => x.HasValue).Value;
            clone.OriginalValue = value.Value;
            return clone;
        }
    }
}
