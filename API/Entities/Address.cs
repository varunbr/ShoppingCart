using System.Collections.Generic;

namespace API.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string House { get; set; }
        public string Landmark { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string PostalCode { get; set; }
        public User User { get; set; }
        public Store Store { get; set; }
        public ICollection<Track> TracksFrom { get; set; }
    }
}
