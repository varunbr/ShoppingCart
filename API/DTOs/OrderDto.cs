using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class BaseOrderDto
    {
        public int Id { get; set; }
        public DateTime Update { get; set; }
        public string Status { get; set; }
    }

    public class UserOrderDto : BaseOrderDto
    {
        public DateTime Delivery { get; set; }
        public DateTime Created { get; set; }
        public double TotalAmount { get; set; }
        public double DeliveryCharge { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }
    }

    public class UserOrderDetailDto : UserOrderDto
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string House { get; set; }
        public string Landmark { get; set; }
        public string PostalCode { get; set; }
        public string LocationName { get; set; }
        public int? TransactionId { get; set; }
        public BaseTrackEventDto CurrentEvent { get; set; }
        public ICollection<BaseTrackEventDto> TrackEvents { get; set; }
    }

    public class StoreOrderDto : BaseOrderDto
    {
        public DateTime Created { get; set; }
        public double TotalAmount { get; set; }
        public double DeliveryCharge { get; set; }
        public int? TransactionId { get; set; }
        public string StoreName { get; set; }
        public string StoreId { get; set; }
        public int SourceLocationId { get; set; }
        public string SourceLocationName { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }
    }

    public class TrackOrderDto : BaseOrderDto
    {
        public BaseTrackEventDto CurrentEvent { get; set; }
        public ICollection<BaseTrackEventDto> TrackEvents { get; set; }
    }

    public class TrackOrderDetailDto : BaseOrderDto
    {
        public DateTime Created { get; set; }
        public DateTime Delivery { get; set; }
        public double TotalAmount { get; set; }
        public double DeliveryCharge { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string House { get; set; }
        public string Landmark { get; set; }
        public string PostalCode { get; set; }
        public string LocationName { get; set; }
        public TrackEventDto CurrentEvent { get; set; }
        public ICollection<TrackEventDto> TrackEvents { get; set; }
    }

    public class TrackRequestDto
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int LocationId { get; set; }
    }

    public class BaseTrackEventDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationType { get; set; }
        public int? AgentId { get; set; }
        public bool Done { get; set; }
    }

    public class TrackEventDto : BaseTrackEventDto
    {
        public string AgentName { get; set; }
        public string AgentUserName { get; set; }
        public string AgentPhotoUrl { get; set; }
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
