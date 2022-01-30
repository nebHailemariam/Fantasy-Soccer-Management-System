using System.Linq.Expressions;
using System.Net;
using API.Enums;

namespace API.Helpers.Filter.ExpressionProviders
{
    public class PlayerPositionFilterExpressionProvider : DefaultFilterExpressionProvider
    {
        public override ConstantExpression GetValue(string input)
        {
            if (!Enum.TryParse(input, out PlayerPositions value))
                throw new AppException($"Invalid search value. Could not parse '{input}' as Player Position", statusCode: HttpStatusCode.BadRequest);

            return Expression.Constant(value);
        }
    }
}