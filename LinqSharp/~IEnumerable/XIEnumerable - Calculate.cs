// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using NStandard.UnitValues;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    //public static partial class XIEnumerable
    //{
    //    public static T Calculate<T>(this IEnumerable<T> source, Action<Ref<T>, IEnumerable<T>> resultSetter) where T : struct
    //    {
    //        var instance = Ref.New<T>();
    //        if (instance.Struct is IUnitValue unitValue) unitValue.Initialize();

    //        if (!source.Any()) return instance;

    //        resultSetter(instance, source);
    //        return instance;
    //    }

    //    public static T? Calculate<T>(this IEnumerable<T?> source, Action<Ref<T>, IEnumerable<T>> resultSetter) where T : struct
    //    {
    //        var instance = Ref.New<T>();
    //        if (instance.Struct is IUnitValue unitValue) unitValue.Initialize();

    //        if (!source.Any()) return instance;
    //        if (source.All(x => !x.HasValue)) return instance;

    //        resultSetter(instance, from item in source where item.HasValue let value = item.Value select value);
    //        return instance;
    //    }

    //}

}
