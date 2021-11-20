using System.Collections.Generic;

namespace API.Entities
{
    public class StoreItem
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int MaxPerOrder { get; set; }
        public int Count { get; set; }
        public bool Available { get; set; }
        public string ConcurrencyStamp { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
