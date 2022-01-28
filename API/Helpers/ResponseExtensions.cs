using System.Text.Json;

namespace API.Helpers
{
    public static class ResponseExtensions
    {
        public static void AddPagination<T>(this HttpResponse response, ref PagedList<T> pagedListObject)
        {
            var paginationHeader = new
            {
                pagedListObject.CurrentPage,
                pagedListObject.TotalPages,
                pagedListObject.PageSize,
                pagedListObject.TotalCount,
                pagedListObject.HasPrevious,
                pagedListObject.HasNext
            };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
