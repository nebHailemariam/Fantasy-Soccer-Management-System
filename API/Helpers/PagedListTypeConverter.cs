using AutoMapper;

namespace API.Helpers
{
    public class PagedListTypeConverter<TSource, TDestination>
    : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
    {
        private readonly IMapper _mapper;
        public PagedListTypeConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            destination = new();
            var destinationItems = _mapper.Map<List<TDestination>>(source);
            destination.AddRange(destinationItems);

            destination.CurrentPage = source.CurrentPage;
            destination.TotalPages = source.TotalPages;
            destination.PageSize = source.PageSize;
            destination.TotalCount = source.TotalCount;

            return destination;
        }
    }
}
