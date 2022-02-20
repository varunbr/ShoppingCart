using System.Collections.Generic;

namespace API.Entities
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PhotoId { get; set; }
        public Photo Photo { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public ICollection<StoreItem> Items { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<StoreAgent> StoreAgents { get; set; }
    }
}
