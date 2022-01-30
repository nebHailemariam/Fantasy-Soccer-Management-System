using API.Helpers.Filter.ExpressionProviders;

namespace API.Helpers.Filter.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FilterAttribute : Attribute
    {
        public string EntityPropertyName { get; set; }

        public IFilterExpressionProvider ExpressionProvider { get; set; }
            = new DefaultFilterExpressionProvider();
    }
}