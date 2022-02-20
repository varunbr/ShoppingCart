using API.DTOs;
using API.Helpers;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class PayRepository : BaseRepository, IPayRepository
    {
        public PayRepository(DataContext dataContext, IMapper mapper, IPhotoService photoService) : base(dataContext, mapper, photoService)
        {
        }

        public async Task<List<PayOptionDto>> GetPaymentOptions(int userId)
        {
            var payOptions = await DataContext.PayOptions.ProjectTo<PayOptionDto>(Mapper.ConfigurationProvider)
                .ToListAsync();

            var wallet = payOptions.First(p => p.Name == Constants.ShoppingCartWallet);
            var walletBalance = await DataContext.Accounts.Where(a => a.User.Id == userId).Select(a => a.Balance).FirstAsync();
            wallet.Balance = walletBalance;
            return payOptions;
        }

        public async Task<Response<TransactionDto, TransactionContext>> GetTransactions(int userId, BaseParams @params)
        {
            var transactions = DataContext.Transactions
                .Where(t => t.FromAccount.User.Id == userId || t.ToAccount.User.Id == userId)
                .OrderByDescending(t => t.Date)
                .ProjectTo<TransactionDto>(Mapper.ConfigurationProvider)
                .AsNoTracking();

            var tParams = Mapper.Map<TransactionContext>(@params);
            tParams.Balance = await DataContext.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Account.Balance)
                .FirstAsync();

            return await Response<TransactionDto, TransactionContext>.CreateAsync(transactions, tParams);
        }

        public async Task TransferAmount(int userId, TransferDto transfer)
        {
            var receiverId = await GetUserIdByUserName(transfer.UserName);
            if (receiverId == 0)
                throw new HttpException("Invalid receiver.");
            if (transfer.Amount <= 0)
                throw new HttpException("Amount should be greater than Zero");

            var from = await DataContext.Users.Where(u => u.Id == userId).Select(u => u.AccountId).FirstAsync();
            var to = await DataContext.Users.Where(u => u.Id == receiverId).Select(u => u.AccountId).FirstAsync();

            await using var transaction = await DataContext.Database.BeginTransactionAsync();
            try
            {
                var accTransaction = await ProcessTransaction(from, to, transfer.Amount, transfer.Description);
                accTransaction.Type = TransactionType.AmountTransfer;
                await DataContext.Transactions.AddAsync(accTransaction);
                if (!await SaveChanges()) throw new HttpException("Failed to transfer.");
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                await SaveChanges();
                throw;
            }
        }
    }
}
