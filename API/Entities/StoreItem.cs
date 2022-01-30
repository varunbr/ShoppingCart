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
        public int SoldQuantity { get; set; }
        public int Available { get; set; }
        public byte[] RowVersion { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
