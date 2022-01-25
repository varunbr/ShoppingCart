using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AddressDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string House { get; set; }
        [Required]
        public string Landmark { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public int AreaId { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public LocationListDto Locations { get; set; }
    }
}
