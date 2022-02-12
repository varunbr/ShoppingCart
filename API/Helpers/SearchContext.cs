using API.Entities;
using API.Extensions;
using System;
using System.Collections.Generic;

namespace API.Helpers
{
    public class SearchContext : BaseParams
    {
        public Dictionary<string, string> QueryParams { get; }
        public string SearchText { get; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public int? PriceFrom { get; }
        public int? PriceTo { get; }
        public string[] Keywords { get; }
        public Dictionary<string, string> Filters { get; }
        public List<Property> Properties { get; set; }

        public SearchContext(Dictionary<string, string> queryParams) : base(queryParams)
        {
            QueryParams = queryParams ?? new Dictionary<string, string>();
            QueryParams.TryGetValue("q", out var searchText);
            SearchText = searchText?.Trim() ?? string.Empty;
            Keywords = SearchText.ToLower().SubWords();
            QueryParams.TryGetValue(Constants.Category, out var category);
            Category = category;
            QueryParams.TryGetValue(Constants.Brand, out var brand);
            Brand = brand;
            if (QueryParams.TryGetValue(Constants.Price, out var range) && range.IsValidIntegerRange())
            {
                range.GetRange(out int? from, out var to);
                PriceFrom = from;
                PriceTo = to;
            }
            Filters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string GetPrice(int? from, int? to)
        {
            return from != null || to != null ? $"{from}-{to}" : null;
        }

    }

    public class HomeContext
    {
        public int ItemsPerCategory { get; set; } = 12;
        public List<string> Categories { get; set; }
    }

    public class TransactionContext : BaseParams
    {
        public double Balance { get; set; }
    }
}
