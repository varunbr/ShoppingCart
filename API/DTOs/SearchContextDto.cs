using System.Collections.Generic;

namespace API.DTOs
{
    public class SearchContextDto
    {
        public Dictionary<string, string> Filters { get; set; }
        public string SearchText { get; set; }
    }
}
