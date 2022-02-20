using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;

namespace API.Data;

public interface IStoreRepository
{
    Task<Response<StoreOrderDto, OrderParams>> GetOrders(OrderParams orderParams, int userId);
    Task<StoreOrderDto> GetOrder(int userId, int orderId);
    Task<StoreOrderDto> StartDispatchingOrder(int userId, int orderId);
}