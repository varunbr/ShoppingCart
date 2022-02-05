using System.Collections.Generic;
using API.DTOs;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;

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
            wallet.Balance= walletBalance;
            return payOptions;
        }
    }
}
