using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using API.Entities;

namespace API.Helpers.Filter.ExpressionProviders
{
    public class StringContainsFilterExpressionProvider : IFilterExpressionProvider
    {
        protected const string ContainsOperator = "cnts";

        public virtual IEnumerable<string> GetOperators()
        {
            yield return ContainsOperator;
        }

        // https://stackoverflow.com/questions/278684/how-do-i-create-an-expression-tree-to-represent-string-containsterm-in-c
        public virtual Expression GetComparison<T>(MemberExpression left, string op, ConstantExpression right)
        {
            switch (op.ToLower())
            {
                case ContainsOperator:
                    Console.WriteLine("\n\n\n\n\n\n");

                    var player = new Player()
                    {
                        FirstName = "Terry henery"
                    };

                    Console.WriteLine(left.Expression.GetType());
                    Console.WriteLine(left.Expression.Type.ToString());
                    Console.WriteLine(left.Member);
                    Console.WriteLine(left.Member.Name);

                    ParameterExpression parameterExp = Expression.Parameter(left.Expression.Type);
                    MemberExpression propertyExp = Expression.Property(parameterExp, left.Member.Name);
                    MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    MethodCallExpression containsMethodExp = Expression.Call(propertyExp, method, right);

                    Console.WriteLine(Expression.Lambda<Func<Player, bool>>(containsMethodExp, parameterExp).Compile()(player));
                    Expression x = containsMethodExp;
                    return x;
                default:
                    break;
            }
            throw new AppException($"Invalid operator '{op}'.", statusCode: HttpStatusCode.BadRequest);
        }

        public virtual ConstantExpression GetValue(string input)
            => Expression.Constant(input);
    }
}