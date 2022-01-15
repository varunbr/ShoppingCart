using System.Collections.Generic;

namespace API.Entities
{
    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Values { get; set; }
        public string Unit { get; set; }
        public bool Filter { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<PropertyValue> PropertyValues { get; set; }
    }
}
