using System.Collections.Generic;

namespace API.DTOs
{
    public class CheckoutDto
    {
        public string AddressName { get; set; }
        public double Price { get; set; }
        public double DeliveryCharge { get; set; }
        public double Total { get; set; }
        public string StoreName { get; set; }
        public int StoreId { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public ICollection<CheckoutItem> Items { get; set; }
    }

    public class CheckoutItem
    {
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public double AmountPerUnit { get; set; }
        public double Total { get; set; }
        public int ProductId { get; set; }
        public int MaxPerOrder { get; set; }
        public int StoreItemId { get; set; }
        public int ItemQuantity { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class CartItemDto
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public int StoreItemId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string PhotoUrl { get; set; }
        public int MaxPerOrder { get; set; }
        public string StoreName { get; set; }
        public int Available { get; set; }
    }

    public class CartStoreDto
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
}
