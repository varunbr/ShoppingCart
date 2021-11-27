namespace API.Entities
{
    public class ProductView
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int PhotoId { get; set; }
        public Photo Photo { get; set; }
        public bool IsMain { get; set; }
    }
}
