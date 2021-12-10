using API.Extensions;
using System.Collections.Generic;

namespace API.Helpers
{
    public class SearchContext : PageParams
    {
        public SearchContext(Dictionary<string, string> queryParams) : base(queryParams)
        {
            QueryParams = queryParams ?? new Dictionary<string, string>();
            QueryParams.TryGetValue("q", out var searchText);
            SearchText = searchText?.Trim() ?? string.Empty;
            Keywords = SearchText.ToLower().SubWords();
            QueryParams.TryGetValue("OrderBy", out var orderBy);
            OrderBy = orderBy;
            Filters = new Dictionary<string, string>();
            Properties = new Dictionary<string, string>();
        }

        public Dictionary<string, string> QueryParams { get; }
        public string OrderBy { get; }
        public string SearchText { get; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string[] Keywords { get; }
        public Dictionary<string, string> Filters { get; }
        public Dictionary<string, string> Properties { get; }
    }
}
