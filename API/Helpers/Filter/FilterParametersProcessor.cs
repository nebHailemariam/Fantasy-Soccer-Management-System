using System.Net;
using System.Reflection;
using API.Helpers.Filter.Attributes;

namespace API.Helpers.Filter
{
    public class FilterParametersProcessor<T>
    {
        private readonly string[] _filterQuery;

        public FilterParametersProcessor(string[] filterQuery)
        {
            _filterQuery = filterQuery;
        }

        public IEnumerable<FilterTerm> GetAllTerms()
        {
            if (_filterQuery == null) yield break;

            foreach (var expression in _filterQuery)
            {
                if (string.IsNullOrWhiteSpace(expression)) continue;

                var tokens = expression.Split(' ');

                if (tokens.Length == 0)
                {
                    yield return new FilterTerm()
                    {
                        Name = expression,
                        ValidSyntax = false
                    };

                    continue;
                }

                if (tokens.Length < 3)
                {
                    yield return new FilterTerm()
                    {
                        Name = tokens[0],
                        ValidSyntax = false
                    };

                    continue;
                }

                yield return new FilterTerm()
                {
                    Name = tokens[0],
                    Operator = tokens[1],
                    Value = string.Join(" ", tokens.Skip(2)),
                    ValidSyntax = true
                };
            }
        }

        public void Validate()
        {
            var validTerms = GetValidTerms().Select(t => t.Name);
            var invalidTerms = GetAllTerms().Select(t => t.Name)
                .Except(validTerms, StringComparer.OrdinalIgnoreCase);

            foreach (var term in invalidTerms)
            {
                throw new AppException($"Invalid filter term '{term}'.", statusCode: HttpStatusCode.BadRequest);
            }
        }

        public IQueryable<T> Apply(IQueryable<T> query)
        {
            var terms = GetValidTerms().ToArray();
            if (!terms.Any()) return query;

            var modifiedQuery = query;

            foreach (var term in terms)
            {
                var propertyInfo = ExpressionHelper
                    .GetPropertyInfo<T>(term.Name);
                var obj = ExpressionHelper.Parameter<T>();

                var lhs = ExpressionHelper.GetPropertyExpression(obj, propertyInfo);
                var rhs = term.ExpressionProvider.GetValue(term.Value);
                var comparisonExpression = term.ExpressionProvider
                    .GetComparison(lhs, term.Operator, rhs);
                var lambdaExpression = ExpressionHelper
                    .GetLambda<T, bool>(obj, comparisonExpression);

                modifiedQuery = ExpressionHelper.CallWhere(modifiedQuery, lambdaExpression);
            }

            return modifiedQuery;
        }

        public IEnumerable<FilterTerm> GetValidTerms()
        {
            var queryTerms = GetAllTerms()
                .Where(t => t.ValidSyntax)
                .ToArray();

            if (!queryTerms.Any()) yield break;

            var declaredTerms = GetTermsFromModel();

            foreach (var term in queryTerms)
            {
                var declaredTerm = declaredTerms
                    .SingleOrDefault(t => t.Name.Equals(term.Name, StringComparison.OrdinalIgnoreCase));
                if (declaredTerm == null) continue;

                yield return new FilterTerm
                {
                    Name = declaredTerm.Name,
                    Operator = term.Operator,
                    Value = term.Value,
                    ValidSyntax = term.ValidSyntax,
                    ExpressionProvider = declaredTerm.ExpressionProvider
                };
            }
        }

        private static IEnumerable<FilterTerm> GetTermsFromModel()
            => typeof(T).GetTypeInfo()
            .DeclaredProperties
            .Where(p => p.GetCustomAttributes<FilterAttribute>().Any())
            .Select(p =>
            {
                var attribute = p.GetCustomAttribute<FilterAttribute>();
                return new FilterTerm
                {
                    Name = p.Name,
                    ExpressionProvider = attribute.ExpressionProvider
                };
            });
    }
}