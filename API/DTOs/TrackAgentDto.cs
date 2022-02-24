namespace API.DTOs
{
    public class TrackAgentDto
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationType { get; set; }
        public int? ParentLocationId { get; set; }
        public string ParentLocationName { get; set; }
        public string ParentLocationType { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }
        public string Role { get; set; }
    }
}
