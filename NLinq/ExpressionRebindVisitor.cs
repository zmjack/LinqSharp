using System.Linq.Expressions;

namespace NLinq
{
    public class ExpressionRebindVisitor : ExpressionVisitor
    {
        private readonly Expression _from, _to;
        public ExpressionRebindVisitor(Expression from, Expression to)
        {
            _from = from;
            _to = to;
        }

        public override Expression Visit(Expression node) => node == _from ? _to : base.Visit(node);
    }
}
