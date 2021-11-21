using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Data
{
    public interface IUserRepository
    {
        Task<int> GetUserIdByUserName(string userName);
        Task<string> GetUserNameById(int id);
        Task<bool> UserExist(int id);
        Task<bool> UserExist(string userName);
        Task<UserProfileDto> GetProfile(int id);
    }
}
