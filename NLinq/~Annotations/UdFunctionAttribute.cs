using System;

namespace NLinq
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class UdFunctionAttribute : Attribute
    {
        public DatabaseProviderName ProviderName { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }

        public UdFunctionAttribute(DatabaseProviderName providerName, string name, string schema = null)
        {
            ProviderName = providerName;
            Name = name;
            Schema = schema;
        }
    }

}
