namespace API.Entities
{
    public class PayOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Available { get; set; }
    }
}
