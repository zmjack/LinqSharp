using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LinqSharp.EFCore.Navigation
{
    public struct PropertyPath
    {
        public bool CastManyToSingle { get; set; }
        public Type EntityType { get; set; }
        public Type PropertyType { get; set; }
        public LambdaExpression NavigationPropertyPath { get; set; }
    }
}
