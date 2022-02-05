using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class UserOrderDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Delivery { get; set; }
        public double TotalAmount { get; set; }
        public double DeliveryCharge { get; set; }
        public string Status { get; set; }
        public int? TransactionId { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public int StoreItemId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
    }

    public class OrderRequestDto
    {
        public string PayOption { get; set; }
        public double TotalAmount { get; set; }
        public int StoreId { get; set; }
        public List<OrderRequestItem> Items { get; set; }

    }

    public class OrderRequestItem
    {
        public int StoreItemId { get; set; }
        public int ItemQuantity { get; set; }
        public double Price { get; set; }
    }
}
