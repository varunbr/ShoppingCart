using System.Collections.Generic;

namespace API.Entities
{
    public class Track
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int FromAddressId { get; set; }
        public Address FromAddress { get; set; }
        public int ToAddressId { get; set; }
        public Address ToAddress { get; set; }
        public ICollection<TrackEvent> Events { get; set; }
    }
}
