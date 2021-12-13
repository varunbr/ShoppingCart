using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using AutoMapper;

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
                .Include(a=>a.Location.Parent.Parent.Parent)
                .Select(a => a.Location)
                .FirstOrDefaultAsync();
        }

        #endregion
    }
}
