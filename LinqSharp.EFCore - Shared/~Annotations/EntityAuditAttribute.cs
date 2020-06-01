using System;

namespace LinqSharp.EFCore
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAuditAttribute : Attribute
    {
        public Type EntityAuditType { get; set; }

        public EntityAuditAttribute(Type entityAuditType)
        {
            EntityAuditType = entityAuditType;
        }
    }

}
