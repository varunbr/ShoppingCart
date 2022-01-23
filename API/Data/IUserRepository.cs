using API.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Data
{
    public interface IUserRepository
    {
        Task<int> GetUserIdByUserName(string userName);
        Task<string> GetUserNameById(int id);
        Task<bool> UserExist(int id);
        Task<bool> UserExist(string userName);
        Task<UserProfileDto> GetProfile(int id);
        Task<AddressDto> GetAddress(int userId);
        Task UpdateAddress(int userId, AddressDto address);
        Task RemoveAddress(int userId);
        Task<string> UpdateUserPhoto(IFormFile file, int userId);
        Task DeleteUserPhoto(int userId);
    }
}
