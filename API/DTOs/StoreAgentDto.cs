namespace API.DTOs
{
    public class StoreAgentDto : BaseAgentDto
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
    }
}
