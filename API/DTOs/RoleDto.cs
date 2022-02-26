namespace API.DTOs
{
    public class BaseRoleDto
    {
        public int UserId { get; set; }
        public string Role { get; set; }
    }

    public class TrackRoleDto : BaseRoleDto
    {
        public int LocationId { get; set; }
    }

    public class StoreRoleDto:BaseRoleDto
    {
        public int StoreId { get; set; }
    }
}
