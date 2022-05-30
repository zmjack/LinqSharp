// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Castle.DynamicProxy;
using NStandard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LinqSharp.EFCore
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EntityAgentProxy<TKvEntityAgent> : IInterceptor where TKvEntityAgent : EntityAgent
    {
        public void Intercept(IInvocation invocation)
        {
            var proxy = (TKvEntityAgent)invocation.Proxy;

            string property;
            string value;
            KvEntity store;
            PropertyInfo proxyProperty;

            if (proxy.IsProxyLoaded())
            {
                switch (invocation.Method.Name)
                {
                    case string name when name.StartsWith("set_"):
                        property = invocation.Method.Name.Substring(4);
                        value = invocation.Arguments[0]?.ToString();
                        store = proxy.GetColumnStores().FirstOrDefault(x => x.Key == property);
                        proxyProperty = proxy.GetType().GetProperty(property);

                        if (store is null) throw new KeyNotFoundException();

                        store.Value = value;
                        break;

                    case string name when name.StartsWith("get_"):
                        property = invocation.Method.Name.Substring(4);
                        store = proxy.GetColumnStores().FirstOrDefault(x => x.Key == property);
                        proxyProperty = proxy.GetType().GetProperty(property);

                        if (store != null)
                        {
                            if (store.Value is null)
                            {
                                invocation.ReturnValue = proxyProperty.PropertyType.CreateDefault();
                            }
                            else
                            {
                                try
                                {
#if NET6_0_OR_GREATER
                                    //TODO: DateOnly and TimeOnly can not use Convert.
                                    object returnValue = proxyProperty.PropertyType switch
                                    {
                                        Type type when type == typeof(DateOnly) => DateOnly.TryParse(store.Value, out var date) ? date : default,
                                        Type type when type == typeof(TimeOnly) => TimeOnly.TryParse(store.Value, out var time) ? time : default,

                                        Type type when type == typeof(DateOnly?) => DateOnly.TryParse(store.Value, out var date) ? (DateOnly?)date : default,
                                        Type type when type == typeof(TimeOnly?) => TimeOnly.TryParse(store.Value, out var time) ? (TimeOnly?)time : default,

                                        _ => ConvertEx.ChangeType(store.Value, proxyProperty.PropertyType),
                                    };
#else
                                    var returnValue = ConvertEx.ChangeType(store.Value, proxyProperty.PropertyType);
#endif
                                    invocation.ReturnValue = returnValue;
                                }
                                catch
                                {
                                    invocation.ReturnValue = proxyProperty.PropertyType.CreateDefault();
                                }
                            }
                        }
                        else invocation.Proceed();
                        break;

                    default: invocation.Proceed(); break;
                }
            }
            else invocation.Proceed();
        }
    }

}
