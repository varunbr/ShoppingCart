using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;

namespace API.Data;

public interface ITrackRepository
{
    Task<Response<TrackOrderDto, TrackParams>> GetOrders(int userId, TrackParams trackParams);
    Task<TrackOrderDetailDto> GetOrder(int orderId);
    Task<TrackOrderDetailDto> ReceiveOrder(int userId, int orderId, int locationId);
    Task<TrackOrderDetailDto> DispatchOrder(int userId, int orderId, int locationId);
    Task<TrackOrderDetailDto> DispatchOrderForDelivery(int userId, int orderId, int locationId);
}