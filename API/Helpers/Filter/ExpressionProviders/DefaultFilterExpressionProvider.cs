using System.Linq.Expressions;
using System.Net;

namespace API.Helpers.Filter.ExpressionProviders
{
    public class DefaultFilterExpressionProvider : IFilterExpressionProvider
    {
        protected const string EqualsOperator = "eq";

        public virtual IEnumerable<string> GetOperators()
        {
            yield return EqualsOperator;
        }

        public virtual Expression GetComparison<T>(MemberExpression left, string op, ConstantExpression right)
        {
            return op.ToLower() switch
            {
                EqualsOperator => Expression.Equal(left, right),
                _ => throw new AppException($"Invalid operator '{op}'.", statusCode: HttpStatusCode.BadRequest),
            };
        }

        public virtual ConstantExpression GetValue(string input)
            => Expression.Constant(input);
    }
}