using System.Linq.Expressions;
using System.Net;

namespace API.Helpers.Filter.ExpressionProviders
{
    public class IntFilterExpressionProvider : ComparableFilterExpressionProvider
    {
        public override ConstantExpression GetValue(string input)
        {
            if (!int.TryParse(input, out var value))
                throw new AppException($"Invalid search value. Could not parse '{input}' as int", statusCode: HttpStatusCode.BadRequest);

            return Expression.Constant(value);
        }
    }
}