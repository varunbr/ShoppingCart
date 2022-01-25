using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(DataContext dataContext, IMapper mapper, IPhotoService photoService) : base(dataContext, mapper, photoService)
        {

        }

        public async Task<UserProfileDto> GetProfile(int id)
        {
            var user = await DataContext.Users
                .Where(u => u.Id == id)
                .ProjectTo<UserProfileDto>(Mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<AddressDto> GetAddress(int userId)
        {
            var address = await DataContext.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Address)
                .ProjectTo<AddressDto>(Mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (address != null)
            {
               address.Locations = await GetLocations(address.AreaId);
            }
            return address;
        }

        public async Task UpdateAddress(int userId, AddressDto addressDto)
        {
            if (!await DataContext.Locations.AnyAsync(l => l.Id == addressDto.AreaId &&
                                                           l.Type.Equals(LocationType.Area.ToString())))
                throw new HttpException("Invalid location Id of Area.");

            var user = await DataContext.Users.Include(u => u.Address).SingleAsync(u => u.Id == userId);
            user.Address = new Address();
            user.Address = Mapper.Map(addressDto, user.Address);
            user.Address.LocationId = addressDto.AreaId;
        }

        public async Task RemoveAddress(int userId)
        {
            var address = await DataContext.Addresses
                .Where(a => a.User.Id == userId)
                .SingleOrDefaultAsync();
            if (address != null)
                DataContext.Addresses.Remove(address);
        }

        public async Task<string> UpdateUserPhoto(IFormFile file, int userId)
        {
            var photo = await DataContext.Photos.FirstOrDefaultAsync(p => p.User.Id == userId) ?? new Photo
            {
                User = new User { Id = userId }
            };
            var publicId = photo.PublicId;
            await UpdatePhoto(file, photo, true);
            if (!await SaveChanges())
                throw new HttpException("Failed to update photo.", StatusCodes.Status500InternalServerError);

            await DeletePhoto(publicId);
            return photo.Url;
        }

        public async Task DeleteUserPhoto(int userId)
        {
            var photo = await DataContext.Photos.FirstOrDefaultAsync(p => p.User.Id == userId);
            if (photo?.Url == null) throw new HttpException("Photo already removed!");
            await DeletePhoto(photo);
        }
    }
}
