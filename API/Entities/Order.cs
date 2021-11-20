using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Created { get; set; }
        public DateTime Delivery { get; set; }
        public double TotalAmount { get; set; }
        public double DeliveryCharge { get; set; }
        public string Status { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public Track Track { get; set; }
    }
}
