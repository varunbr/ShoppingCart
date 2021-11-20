namespace API.Entities
{
    public class TrackEvent
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public Track Track { get; set; }
        public int LocationId { get; set; }
        public Location SiteLocation { get; set; }
        public int UserId { get; set; }
        public User Agent { get; set; }
        public string Status { get; set; }
    }
}
