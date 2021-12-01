using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AddressDto
    {
        [Required]
        public string House { get; set; }
        [Required]
        public string Landmark { get; set; }
        [Required]
        public int LocationId { get; set; }
        [Required]
        public string PostalCode { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
