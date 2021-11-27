using System.Collections.Generic;

namespace API.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public byte[] RowVersion { get; set; }
        public User User { get; set; }
        public Store Store { get; set; }
        public ICollection<Transaction> Withdraw { get; set; }
        public ICollection<Transaction> Deposit { get; set; }
    }
}
