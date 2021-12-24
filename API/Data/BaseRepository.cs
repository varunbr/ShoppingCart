using API.Entities;
using API.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class BaseRepository
    {
        public DataContext DataContext { get; }
        public IMapper Mapper { get; }

        public BaseRepository(DataContext dataContext, IMapper mapper)
        {
            DataContext = dataContext;
            Mapper = mapper;
        }

        #region User

        public async Task<string> GetUserNameById(int id)
        {
            return await DataContext.Users.Where(u => u.Id == id)
                .Select(u => u.UserName)
                .SingleOrDefaultAsync();
        }

        public async Task<int> GetUserIdByUserName(string userName)
        {
            return await DataContext.Users.Where(u => u.UserName == userName.ToLower())
                .Select(u => u.Id)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> UserExist(int id)
        {
            return await DataContext.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> UserExist(string userName)
        {
            return await DataContext.Users.AnyAsync(u => u.UserName == userName.ToLower());
        }

        public async Task<Location> GetUserLocation(int userId)
        {
            if (userId == 0)
                return null;

            return await DataContext.Addresses.Where(a => a.User.Id == userId)
                .Include(a => a.Location.Parent.Parent.Parent)
                .Select(a => a.Location)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UserHasAddress(int userId)
        {
            if (userId == 0) return false;
            return await DataContext.Users.AnyAsync(u => u.AddressId != null);
        }

        #endregion

        #region Product

        public async Task<bool> IsInterStateDelivery(int userId, int storeId)
        {
            if (userId == 0) return false;

            var userLocationId = await DataContext.Addresses.Where(a => a.User.Id == userId)
                .Select(a => a.Location.Parent.ParentId)
                .FirstOrDefaultAsync();

            return await DataContext.Stores
                .Where(s => s.Id == storeId && s.Address.Location.Parent.ParentId == userLocationId)
                .AnyAsync();
        }

        #endregion

        #region Account

        public async Task<Transaction> ProcessTransaction(int from, int to, double amount, string description)
        {
            var transaction = new Transaction
            {
                Amount = amount,
                Date = DateTime.UtcNow,
                Description = description,
                FromId = from,
                ToId = to
            };

            var fromAcc = await DataContext.Accounts.SingleAsync(a => a.Id == from);
            var toAcc = await DataContext.Accounts.SingleAsync(a => a.Id == to);

            if (fromAcc.Balance < amount)
                throw new HttpException("Insufficient balance to cover this transactions.");

            fromAcc.Balance -= amount;
            toAcc.Balance += amount;

            return transaction;
        }

        #endregion
    }
}
