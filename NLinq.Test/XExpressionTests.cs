using SimpleData.Northwnd;
using System;
using System.Linq.Expressions;
using Xunit;

namespace NLinq.Test
{
    public class XExpressionTests
    {
        [Fact]
        public void Test()
        {
            var exp = new Expression<Func<OrderDetail, bool>>[]
            {
                 x => x.OrderID == 1,
                 x => x.ProductID == 1,
                 x => x.UnitPrice == 1,
            }.LambdaJoin(Expression.OrElse);
            Assert.Equal("x => (((x.OrderID == 1) OrElse (x.ProductID == 1)) OrElse (x.UnitPrice == 1))", exp.ToString());
        }
    }
}
