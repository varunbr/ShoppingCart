using System.Collections.Generic;

namespace API.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public Category Parent { get; set; }
        public int? PhotoId { get; set; }
        public Photo Photo { get; set; }
        public ICollection<CategoryTag> CategoryTags { get; set; }
        public List<Property> Properties { get; set; }
        public ICollection<Category> Children { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
