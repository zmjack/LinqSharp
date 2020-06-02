using System;

namespace LinqSharp.EFCore
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAuditAttribute : Attribute
    {
        public Type EntityAuditorType { get; set; }

        public EntityAuditAttribute(Type entityAuditorType)
        {
            EntityAuditorType = entityAuditorType;
        }
    }

}
