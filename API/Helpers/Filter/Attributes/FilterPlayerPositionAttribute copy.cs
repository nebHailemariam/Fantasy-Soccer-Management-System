using API.Helpers.Filter.ExpressionProviders;

namespace API.Helpers.Filter.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FilterPlayerPositionAttribute : FilterAttribute
    {
        public FilterPlayerPositionAttribute()
        {
            ExpressionProvider = new PlayerPositionFilterExpressionProvider();
        }
    }
}