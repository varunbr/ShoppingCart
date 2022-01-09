using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;

namespace API.Data;

public interface IOrderRepository
{
    Task<CheckoutDto> CheckOut(int userId, List<CheckoutItem> items);
    Task<int> OrderItems(int userId, List<CheckoutItem> items);
    Task<Response<UserOrderDto, BaseParams>> GetUserOrders(int userId, BaseParams @params);
    Task<UserOrderDto> GetUserOrder(int userId, int orderId);
}