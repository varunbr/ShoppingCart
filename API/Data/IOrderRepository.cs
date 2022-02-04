using API.DTOs;
using API.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Data;

public interface IOrderRepository
{
    Task<CheckoutDto> CheckOut(int userId, List<CheckoutItem> items);
    Task<int> OrderItems(int userId, List<CheckoutItem> items);
    Task<Response<UserOrderDto, BaseParams>> GetUserOrders(int userId, BaseParams @params);
    Task<UserOrderDto> GetUserOrder(int userId, int orderId);
    Task<List<CartStoreDto>> GetCart(int userId);
    Task<CartItemDto> GetCart(int userId, int storeItemId);
    Task<CartItem> AddToCart(int userId, int storeItemId, int productId);
    Task RemoveFromCart(int userId, int[] storeItemIds);
}