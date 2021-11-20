using System.Collections.Generic;

namespace API.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string Features { get; set; }
        public double Amount { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int? PhotoId { get; set; }
        public Photo Photo { get; set; }
        public ICollection<StoreItem> StoreItems { get; set; }
    }
}
