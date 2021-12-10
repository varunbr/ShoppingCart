namespace API.Entities
{
    public class CategoryTag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public short Score { get; set; }
    }
}
