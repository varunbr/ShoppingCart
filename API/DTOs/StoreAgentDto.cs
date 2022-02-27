namespace API.DTOs
{
    public class StoreAgentDto : BaseAgentDto
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
    }

    public class StoreInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
