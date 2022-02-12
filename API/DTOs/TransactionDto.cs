using System;

namespace API.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Amount { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int? OrderId { get; set; }
    }

    public class TransferDto
    {
        public string UserName { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
