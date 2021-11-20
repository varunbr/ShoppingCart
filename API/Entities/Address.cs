using System.Collections.Generic;

namespace API.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Landmark { get; set; }
        public int AreaId { get; set; }
        public Location Area { get; set; }
        public int CityId { get; set; }
        public Location City { get; set; }
        public int StateId { get; set; }
        public Location State { get; set; }
        public int CountryId { get; set; }
        public Location Country { get; set; }
        public string PostalCode { get; set; }
        public User User { get; set; }
        public Store Store { get; set; }
        public ICollection<Track> TracksFrom { get; set; }
        public ICollection<Track> TracksTo { get; set; }
    }
}
