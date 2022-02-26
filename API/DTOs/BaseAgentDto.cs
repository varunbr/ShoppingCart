namespace API.DTOs
{
    public class BaseAgentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }
        public string Role { get; set; }
    }
}
