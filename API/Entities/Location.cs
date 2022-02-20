using System.Collections.Generic;

namespace API.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public Location Parent { get; set; }
        public ICollection<Location> Children { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Order> SourceOrders { get; set; }
        public ICollection<Order> DestinationOrders { get; set; }
        public ICollection<TrackEvent> TrackEvents { get; set; }
        public ICollection<TrackAgent> TrackAgents { get; set; }
    }
}
