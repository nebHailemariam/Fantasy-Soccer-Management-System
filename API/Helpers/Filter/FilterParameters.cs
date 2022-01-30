namespace API.Helpers.Filter
{
    public class FilterParameters<T>
    {
        public string[] Filter { get; set; }

        public IQueryable<T> Apply(IQueryable<T> query)
        {
            var processor = new FilterParametersProcessor<T>(Filter);
            processor.Validate();
            return processor.Apply(query);
        }
    }
}