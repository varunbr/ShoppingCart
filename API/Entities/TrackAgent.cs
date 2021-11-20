namespace API.Entities
{
    public class TrackAgent
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Role { get; set; }
    }
}
