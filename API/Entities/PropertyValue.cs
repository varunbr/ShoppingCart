namespace API.Entities
{
    public class PropertyValue
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public string StringValue { get; set; }
        public int? IntegerValue { get; set; }
    }
}
