namespace API.DTOs
{
    public class TrackRoleDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public int LocationId { get; set; }
    }

    public class StoreRoleDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public int StoreId { get; set; }
    }

    public class AdminRoleDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string Role { get; set; }
    }
}
