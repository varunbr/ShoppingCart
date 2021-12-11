using API.Helpers;

namespace API.DTOs
{
    public class SearchResult
    {
        public SearchContextDto Context { get; }
        public PagedList<ProductDto> Items { get; }

        public SearchResult(PagedList<ProductDto> items, SearchContextDto context)
        {
            Context = context;
            Items = items;
        }
    }
}
