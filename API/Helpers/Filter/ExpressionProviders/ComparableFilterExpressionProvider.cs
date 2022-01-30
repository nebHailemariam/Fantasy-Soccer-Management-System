using System.Linq.Expressions;

namespace API.Helpers.Filter.ExpressionProviders
{
    public class ComparableFilterExpressionProvider : DefaultFilterExpressionProvider
    {
        private const string GreaterThanOperator = "gt";
        private const string GreaterThanEqualToOperator = "gte";
        private const string LessThanOperator = "lt";
        private const string LessThanEqualToOperator = "lte";

        public override IEnumerable<string> GetOperators()
            => base.GetOperators()
            .Concat(new[]
            {
                GreaterThanOperator,
                GreaterThanEqualToOperator,
                LessThanOperator,
                LessThanEqualToOperator
            });

        public override Expression GetComparison<T>(MemberExpression left, string op, ConstantExpression right)
        {
            return op.ToLower() switch
            {
                GreaterThanOperator => Expression.GreaterThan(left, right),
                GreaterThanEqualToOperator => Expression.GreaterThanOrEqual(left, right),
                LessThanOperator => Expression.LessThan(left, right),
                LessThanEqualToOperator => Expression.LessThanOrEqual(left, right),
                _ => base.GetComparison<T>(left, op, right),
            };
        }
    }
}