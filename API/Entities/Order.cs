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
        public DateTime Update { get; set; }
        public DateTime Delivery { get; set; }
        public double TotalAmount { get; set; }
        public double DeliveryCharge { get; set; }
        public string Status { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public int SourceLocationId { get; set; }
        public Location SourceLocation { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public string House { get; set; }
        public string Landmark { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int DestinationLocationId { get; set; }
        public Location DestinationLocation { get; set; }
        public string PostalCode { get; set; }
        public ICollection<TrackEvent> TrackEvents { get; set; }
        public int? TransactionId { get; set; }
        public Transaction Transaction { get; set; }
    }
}
