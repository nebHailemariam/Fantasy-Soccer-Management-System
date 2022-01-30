using API.Helpers.Filter.ExpressionProviders;

namespace API.Helpers.Filter.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FilterStringContainsAttribute : FilterAttribute
    {
        public FilterStringContainsAttribute()
        {
            ExpressionProvider = new StringContainsFilterExpressionProvider();
        }
    }
}