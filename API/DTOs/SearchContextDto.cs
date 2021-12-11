using System.Collections.Generic;

namespace API.DTOs
{
    public class SearchContextDto
    {
        public string SearchText { get; set; }
        public string Price { get; set; }
        public string OrderBy { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public Dictionary<string, string> Filters { get; set; }
        public List<PropertyDto> Properties { get; set; }
    }
}
