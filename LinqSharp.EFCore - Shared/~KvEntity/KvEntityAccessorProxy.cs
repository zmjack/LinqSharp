using Castle.DynamicProxy;
using NStandard;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LinqSharp.EFCore
{
    [EditorBrowsable(EditorBrowsableState.Never)]
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
                        value = invocation.Arguments[0]?.ToString();
                        store = proxy.GetColumnStores().FirstOrDefault(x => x.Key == property);
                        proxyProperty = proxy.GetType().GetProperty(property);

                        if (store != null)
                            store.Value = value;
                        else throw new KeyNotFoundException();
                        break;

                    case string name when name.StartsWith("get_"):
                        property = invocation.Method.Name.Substring(4);
                        store = proxy.GetColumnStores().FirstOrDefault(x => x.Key == property);
                        proxyProperty = proxy.GetType().GetProperty(property);

                        if (store != null)
                        {
                            try
                            {
                                var ret = ConvertEx.ChangeType(store.Value, proxyProperty.PropertyType);
                                invocation.ReturnValue = ret;
                            }
                            catch
                            {
                                invocation.ReturnValue = proxyProperty.PropertyType.CreateDefault();
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
