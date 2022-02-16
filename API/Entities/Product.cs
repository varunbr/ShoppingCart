using System;
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
        public int MaxPerOrder { get; set; }
        public int SoldQuantity { get; set; }
        public int CategoryId { get; set; }
        public bool Available { get; set; }
        public byte[] RowVersion { get; set; }
        public DateTime Created { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductView> ProductViews { get; set; }
        public ICollection<StoreItem> StoreItems { get; set; }
        public ICollection<PropertyValue> Properties { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
    }
}
