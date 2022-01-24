using System.Collections.Generic;

namespace API.DTOs
{
    public class CategoryMiniDto
    {
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class HomePageDto
    {
        public List<CategoryProductDto> CategoryProducts { get; set; } = new();
        public List<CategoryMiniDto> Categories { get; set; } = new();
    }
}
