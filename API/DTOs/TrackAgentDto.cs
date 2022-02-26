namespace API.DTOs
{
    public class TrackAgentDto : BaseAgentDto
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationType { get; set; }
        public int? ParentLocationId { get; set; }
        public string ParentLocationName { get; set; }
        public string ParentLocationType { get; set; }
    }
}
