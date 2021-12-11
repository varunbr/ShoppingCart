namespace API.Entities
{
    public class ProductTag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public short Score { get; set; }
    }
}
