namespace API.Entities
{
    public class StoreAgent
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Role { get; set; }
    }
}
