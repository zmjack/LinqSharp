using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NLinq
{
    public class KvEntityAccessorProxy<TRegistry> : IInterceptor
       where TRegistry : KvEntityAccessor
    {
        public void Intercept(IInvocation invocation)
        {
            var proxy = (TRegistry)invocation.Proxy;

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
                        value = invocation.Arguments[0].ToString();
                        store = proxy.GetColumnStores().FirstOrDefault(x => x.Key == property);
                        proxyProperty = proxy.GetType().GetProperty(property);

                        if (store != null)
                            store.Value = value.ToString();
                        else throw new KeyNotFoundException();
                        break;

                    case string name when name.StartsWith("get_"):
                        property = invocation.Method.Name.Substring(4);
                        store = proxy.GetColumnStores().FirstOrDefault(x => x.Key == property);
                        proxyProperty = proxy.GetType().GetProperty(property);

                        if (store != null)
                            invocation.ReturnValue = Convert.ChangeType(store.Value, proxyProperty.PropertyType);
                        else invocation.Proceed();
                        break;

                    default: invocation.Proceed(); break;
                }
            }
            else invocation.Proceed();
        }
    }

}
