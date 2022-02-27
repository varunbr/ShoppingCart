using System.Collections.Generic;

namespace API.DTOs
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class LocationListDto
    {
        public int AreaId { get; set; }
        public IEnumerable<LocationDto> Areas { get; set; }
        public IEnumerable<LocationDto> Cities { get; set; }
        public IEnumerable<LocationDto> States { get; set; }
    }

    public class LocationInfoDto : LocationDto
    {
        public string ParentName { get; set; }
    }
}
