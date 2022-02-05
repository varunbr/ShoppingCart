using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Data;

public interface IPayRepository
{
    Task<List<PayOptionDto>> GetPaymentOptions(int userId);
}