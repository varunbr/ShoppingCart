using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Data;

public interface IOrderRepository
{
    Task<CheckoutDto> CheckOut(int userId, List<CheckoutItem> items);
    Task<int> OrderItems(int userId, List<CheckoutItem> items);
}