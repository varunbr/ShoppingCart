using System.Collections.Generic;

namespace API.DTOs
{
    public class CheckoutDto
    {
        public double Price { get; set; }
        public double DeliveryCharge { get; set; }
        public double Total { get; set; }
        public string StoreName { get; set; }
        public int  StoreId { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public ICollection<CheckoutItem> Items { get; set; }
    }

    public class CheckoutItem
    {
        public string Name { get; set; }
        public double AmountPerUnit { get; set; }
        public double Total { get; set; }
        public int StoreItemId { get; set; }
        public int ItemQuantity { get; set; }
        public string ErrorMessage { get; set; }
    }
}
