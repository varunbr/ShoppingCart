namespace API.Entities
{
    public class ProductView
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string PublicId { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }
}
