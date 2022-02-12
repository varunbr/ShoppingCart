using API.DTOs;
using API.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Data;

public interface IPayRepository
{
    Task<List<PayOptionDto>> GetPaymentOptions(int userId);
    Task<Response<TransactionDto, TransactionContext>> GetTransactions(int userId, BaseParams @params);
    Task TransferAmount(int userId, TransferDto transfer);
}