namespace API.DTOs
{
    public class PayOptionDto
    {
        public string Name { get; set; }
        public bool Available { get; set; }
        public string Type { get; set; }
        public double Balance { get; set; }
    }
}