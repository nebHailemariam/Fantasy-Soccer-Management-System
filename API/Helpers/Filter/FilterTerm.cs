namespace API.Helpers.Filter
{
    public class FilterTerm
    {
        public string Name { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public bool ValidSyntax { get; set; }
        public IFilterExpressionProvider ExpressionProvider { get; set; }
    }
}