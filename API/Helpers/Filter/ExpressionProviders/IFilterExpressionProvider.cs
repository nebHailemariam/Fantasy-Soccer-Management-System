using System.Linq.Expressions;

namespace API.Helpers.Filter
{
    public interface IFilterExpressionProvider
    {
        IEnumerable<string> GetOperators();

        ConstantExpression GetValue(string input);

        Expression GetComparison<T>(MemberExpression left, string op, ConstantExpression right);
    }
}