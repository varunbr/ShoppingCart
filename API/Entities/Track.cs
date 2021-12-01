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
        public string House { get; set; }
        public string Landmark { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string PostalCode { get; set; }
        public ICollection<TrackEvent> Events { get; set; }
    }
}
