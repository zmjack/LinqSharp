// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Castle.DynamicProxy;
using LinqSharp.EFCore.Entities;
using NStandard;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LinqSharp.EFCore.Agent
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class KeyValueAgentProxy<TKeyValueAgent, TKeyValueEntity> : IInterceptor
        where TKeyValueAgent : KeyValueAgent<TKeyValueAgent, TKeyValueEntity>, new()
        where TKeyValueEntity : KeyValueEntity, new()
    {
        public void Intercept(IInvocation invocation)
        {
            var proxy = (TKeyValueAgent)invocation.Proxy;

            var property = invocation.Method.Name.Substring(4);
            var entity = proxy._entities.FirstOrDefault(x => x.Key == property);

            if (invocation.Method.Name.StartsWith("set_"))
            {
                if (entity is null) throw new KeyNotFoundException($"The key ({property}) was not found.)");

                var value = invocation.Arguments[0]?.ToString();
                entity.Value = value;
            }
            else if (invocation.Method.Name.StartsWith("get_"))
            {
                if (entity is null)
                {
                    invocation.Proceed();
                    return;
                }

                var proxyProperty = proxy.GetType().GetProperty(property);
                if (entity.Value is null)
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
                            Type type when type == typeof(DateOnly) => DateOnly.TryParse(entity.Value, out var date) ? date : default,
                            Type type when type == typeof(TimeOnly) => TimeOnly.TryParse(entity.Value, out var time) ? time : default,

                            Type type when type == typeof(DateOnly?) => DateOnly.TryParse(entity.Value, out var date) ? (DateOnly?)date : default,
                            Type type when type == typeof(TimeOnly?) => TimeOnly.TryParse(entity.Value, out var time) ? (TimeOnly?)time : default,

                            _ => ConvertEx.ChangeType(entity.Value, proxyProperty.PropertyType),
                        };
#else
                        var returnValue = ConvertEx.ChangeType(entity.Value, proxyProperty.PropertyType);
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
        }
    }

}
