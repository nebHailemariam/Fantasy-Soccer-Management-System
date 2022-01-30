using API.Helpers.Filter.ExpressionProviders;

namespace API.Helpers.Filter.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FilterIntAttribute : FilterAttribute
    {
        public FilterIntAttribute()
        {
            ExpressionProvider = new IntFilterExpressionProvider();
        }
    }
}