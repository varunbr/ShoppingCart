using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        private readonly UserManager<User> _userManager;

        public RoleRepository(DataContext dataContext, IMapper mapper, IPhotoService photoService, UserManager<User> userManager) : base(dataContext, mapper, photoService)
        {
            _userManager = userManager;
        }

        public async Task<Response<BaseAgentDto, BaseRoleParams>> GetModeratorsForAdmin(int userId, BaseRoleParams roleParams)
        {
            if (!await IsAdmin(userId))
                throw new HttpException("You are not Admin", StatusCodes.Status403Forbidden);

            var rolesQuery = DataContext.UserRoles.AsQueryable();


            if (!string.IsNullOrWhiteSpace(roleParams.UserName))
            {
                var inner = PredicateBuilder.False<UserRole>();
                foreach (var name in roleParams.UserName.Split(','))
                {
                    inner = inner.Or(a => a.User.UserName == name.ToLower());
                }
                rolesQuery = rolesQuery.Where(inner);
            }

            rolesQuery = string.IsNullOrWhiteSpace(roleParams.Role)
                ? rolesQuery.Where(r =>
                    new[] { RoleType.Admin.ToString(), RoleType.StoreModerator.ToString(), RoleType.TrackModerator.ToString() }.Contains(r.Role.Name))
                : rolesQuery.Where(r => r.Role.Name == roleParams.Role);

            var roles = rolesQuery
                .ProjectTo<BaseAgentDto>(Mapper.ConfigurationProvider)
                .AsNoTracking();

            return await Response<BaseAgentDto, BaseRoleParams>.CreateAsync(roles, roleParams);
        }

        public async Task<Response<StoreAgentDto, StoreRoleParams>> GetStoreAgentsForStoreAdmin(int userId, StoreRoleParams roleParams)
        {
            var agentsQuery = from agent in DataContext.StoresAgents
                              where agent.UserId == userId && agent.Role == RoleType.StoreAdmin.ToString()
                              join storeAgent in DataContext.StoresAgents on agent.StoreId equals storeAgent.StoreId
                              select storeAgent;

            return await GetStoreAgents(agentsQuery, roleParams);
        }

        public async Task<Response<StoreAgentDto, StoreRoleParams>> GetStoreAgentsForModerator(int userId, StoreRoleParams roleParams)
        {
            if (!await IsStoreModerator(userId))
                throw new HttpException("You are not Moderator", StatusCodes.Status403Forbidden);

            var agentsQuery = DataContext.StoresAgents.AsQueryable();
            return await GetStoreAgents(agentsQuery, roleParams);
        }

        private async Task<Response<StoreAgentDto, StoreRoleParams>> GetStoreAgents(IQueryable<StoreAgent> agentsQuery, StoreRoleParams roleParams)
        {
            if (!string.IsNullOrWhiteSpace(roleParams.UserName))
            {
                var inner = PredicateBuilder.False<StoreAgent>();
                foreach (var name in roleParams.UserName.Split(','))
                {
                    inner = inner.Or(a => a.User.UserName == name.ToLower());
                }
                agentsQuery = agentsQuery.Where(inner);
            }

            if (!string.IsNullOrWhiteSpace(roleParams.StoreName))
            {
                var inner = PredicateBuilder.False<StoreAgent>();

                foreach (var store in roleParams.StoreName.Split(','))
                {
                    inner = inner.Or(a => a.Store.Name.ToLower() == store.ToLower());
                }
                agentsQuery = agentsQuery.Where(inner);
            }

            if (!string.IsNullOrWhiteSpace(roleParams.Role))
                agentsQuery = agentsQuery.Where(a => a.Role == roleParams.Role);

            var agents = agentsQuery
                .ProjectTo<StoreAgentDto>(Mapper.ConfigurationProvider)
                .AsNoTracking();

            return await Response<StoreAgentDto, StoreRoleParams>.CreateAsync(agents, roleParams);

        }

        public async Task<Response<TrackAgentDto, TrackRoleParams>> GetTrackAgentsForTrackAdmin(int userId, TrackRoleParams roleParams)
        {
            var agentsQuery = from agent in DataContext.TrackAgents
                              where agent.UserId == userId && agent.Role == RoleType.TrackAdmin.ToString()
                              join trackAgent in DataContext.TrackAgents on agent.LocationId equals trackAgent.LocationId
                              select trackAgent;

            return await GetTrackAgents(agentsQuery, roleParams);
        }

        public async Task<Response<TrackAgentDto, TrackRoleParams>> GetTrackAgentsFoModerator(int userId, TrackRoleParams roleParams)
        {
            if (!await IsTrackModerator(userId))
                throw new HttpException("You are not Moderator", StatusCodes.Status403Forbidden);

            var agentsQuery = DataContext.TrackAgents.AsQueryable();
            return await GetTrackAgents(agentsQuery, roleParams);
        }

        private async Task<Response<TrackAgentDto, TrackRoleParams>> GetTrackAgents(IQueryable<TrackAgent> agentsQuery,
            TrackRoleParams roleParams)
        {
            if (!string.IsNullOrWhiteSpace(roleParams.UserName))
            {
                var inner = PredicateBuilder.False<TrackAgent>();
                foreach (var name in roleParams.UserName.Split(','))
                {
                    inner = inner.Or(a => a.User.UserName == name.ToLower());
                }
                agentsQuery = agentsQuery.Where(inner);
            }

            if (!string.IsNullOrWhiteSpace(roleParams.Location))
            {
                var addedLocations = new List<string>();
                var inner = PredicateBuilder.False<TrackAgent>();

                foreach (var location in roleParams.Location.Split(','))
                {
                    if (location.TryParseLocation(out var name, out var type))
                    {
                        addedLocations.Add(location);
                        inner = inner.Or(a => a.Location.Name.ToLower() == name.ToLower() && a.Location.Type == type);
                    }
                }

                roleParams.Location = string.Join(',', addedLocations);

                if (!string.IsNullOrWhiteSpace(roleParams.Location))
                    agentsQuery = agentsQuery.Where(inner);
            }

            if (!string.IsNullOrWhiteSpace(roleParams.Role))
                agentsQuery = agentsQuery.Where(a => a.Role == roleParams.Role);

            var agents = agentsQuery
                .ProjectTo<TrackAgentDto>(Mapper.ConfigurationProvider)
                .AsNoTracking();

            return await Response<TrackAgentDto, TrackRoleParams>.CreateAsync(agents, roleParams);
        }

        public async Task<BaseAgentDto> AddModeratorByAdmin(int userId, BaseRoleDto roleDto)
        {
            if (!await IsAdmin(userId))
                throw new HttpException("You are not admin", StatusCodes.Status403Forbidden);

            if (!IsAdminRole(roleDto.Role))
                throw new HttpException($"Invalid role - {roleDto.Role}");

            if (await DataContext.UserRoles.AnyAsync(r => r.Role.Name == roleDto.Role && r.UserId == roleDto.UserId))
                throw new HttpException("User already in this role");

            var user = await DataContext.Users.Where(u => u.Id == roleDto.UserId).FirstOrDefaultAsync();

            if (user == null)
                throw new HttpException("User not found");

            var result = await _userManager.AddToRoleAsync(user, roleDto.Role);
            if (!result.Succeeded)
                throw new HttpException(result.Errors.ToString(), StatusCodes.Status500InternalServerError);

            return await DataContext.UserRoles
                .Where(r => r.UserId == roleDto.UserId && r.Role.Name == roleDto.Role)
                .ProjectTo<BaseAgentDto>(Mapper.ConfigurationProvider)
                .FirstAsync();
        }

        public async Task<TrackAgentDto> AddRoleByTrackAdmin(int userId, TrackRoleDto roleDto)
        {
            if (!await IsTrackLocationAdmin(userId, roleDto.LocationId))
                throw new HttpException("You are not TrackAdmin of this location.", StatusCodes.Status403Forbidden);

            return await AddTrackRole(roleDto);
        }

        public async Task<TrackAgentDto> AddTrackRoleByModerator(int userId, TrackRoleDto roleDto)
        {
            if (!await IsTrackModerator(userId))
                throw new HttpException("You are not Moderator.", StatusCodes.Status403Forbidden);

            if (!await DataContext.Locations.AnyAsync(s => s.Id == roleDto.LocationId))
                throw new HttpException("Invalid Location");

            return await AddTrackRole(roleDto);
        }

        private async Task<TrackAgentDto> AddTrackRole(TrackRoleDto roleDto)
        {
            if (!IsTrackRole(roleDto.Role))
                throw new HttpException($"Invalid role - {roleDto.Role}");

            var user = await DataContext.Users.Where(u => u.Id == roleDto.UserId).FirstOrDefaultAsync();
            if (user == null)
                throw new HttpException("User not found");

            if (await DataContext.TrackAgents.AnyAsync(a => a.UserId == roleDto.UserId && a.LocationId == roleDto.LocationId && a.Role == roleDto.Role))
                throw new HttpException("User already in this role");

            var agent = new TrackAgent
            {
                Role = roleDto.Role,
                LocationId = roleDto.LocationId,
                UserId = roleDto.UserId
            };

            await DataContext.TrackAgents.AddAsync(agent);

            if (!await SaveChanges())
                throw new HttpException("Failed to add role");

            await _userManager.AddToRoleAsync(user, roleDto.Role);

            return await DataContext.TrackAgents
                .Where(a => a.LocationId == roleDto.LocationId && a.UserId == roleDto.UserId && a.Role == roleDto.Role)
                .ProjectTo<TrackAgentDto>(Mapper.ConfigurationProvider)
                .FirstAsync();
        }

        public async Task<StoreAgentDto> AddRoleByStoreAdmin(int userId, StoreRoleDto roleDto)
        {
            if (!await IsStoreAdmin(userId, roleDto.StoreId))
                throw new HttpException("You are not StoreAdmin of this store.", StatusCodes.Status403Forbidden);

            return await AddStoreRole(roleDto);
        }

        public async Task<StoreAgentDto> AddStoreRoleByModerator(int userId, StoreRoleDto roleDto)
        {
            if (!await IsStoreModerator(userId))
                throw new HttpException("You are not Moderator.", StatusCodes.Status403Forbidden);

            if (!await DataContext.Stores.AnyAsync(s => s.Id == roleDto.StoreId))
                throw new HttpException("Invalid Store");

            return await AddStoreRole(roleDto);
        }

        private async Task<StoreAgentDto> AddStoreRole(StoreRoleDto roleDto)
        {
            if (!IsStoreRole(roleDto.Role))
                throw new HttpException($"Invalid role - {roleDto.Role}");

            var user = await DataContext.Users.Where(u => u.Id == roleDto.UserId).FirstOrDefaultAsync();
            if (user == null)
                throw new HttpException("User not found");

            if (await DataContext.StoresAgents.AnyAsync(a => a.UserId == roleDto.UserId && a.StoreId == roleDto.StoreId && a.Role == roleDto.Role))
                throw new HttpException("User already in this role");

            var agent = new StoreAgent
            {
                Role = roleDto.Role,
                StoreId = roleDto.StoreId,
                UserId = roleDto.UserId
            };

            await DataContext.StoresAgents.AddAsync(agent);

            if (!await SaveChanges())
                throw new HttpException("Failed to add role");

            await _userManager.AddToRoleAsync(user, roleDto.Role);

            return await DataContext.StoresAgents
                .Where(a => a.StoreId == roleDto.StoreId && a.UserId == roleDto.UserId && a.Role == roleDto.Role)
                .ProjectTo<StoreAgentDto>(Mapper.ConfigurationProvider)
                .FirstAsync();
        }

        public async Task RemoveModeratorByAdmin(int userId, BaseRoleDto roleDto)
        {
            if (!await IsAdmin(userId))
                throw new HttpException("You are not admin", StatusCodes.Status403Forbidden);

            if (!IsAdminRole(roleDto.Role))
                throw new HttpException($"Invalid role - {roleDto.Role}");

            if (!await DataContext.UserRoles.AnyAsync(r => r.Role.Name == roleDto.Role && r.UserId == roleDto.UserId))
                throw new HttpException("User is not in this role");

            var user = await DataContext.Users.Where(u => u.Id == roleDto.UserId).FirstOrDefaultAsync();

            if (user == null)
                throw new HttpException("User not found");

            var result = await _userManager.RemoveFromRoleAsync(user, roleDto.Role);
            if (!result.Succeeded)
                throw new HttpException(result.Errors.ToString(), StatusCodes.Status500InternalServerError);
        }

        public async Task RemoveRoleByTrackAdmin(int userId, TrackRoleDto roleDto)
        {
            if (!await IsTrackLocationAdmin(userId, roleDto.LocationId))
                throw new HttpException("You are not TrackAdmin of this location.", StatusCodes.Status403Forbidden);

            await RemoveTrackRole(roleDto);
        }

        public async Task RemoveTrackRoleByModerator(int userId, TrackRoleDto roleDto)
        {
            if (!await IsTrackModerator(userId))
                throw new HttpException("You are not Moderator.", StatusCodes.Status403Forbidden);

            if (!await DataContext.Locations.AnyAsync(s => s.Id == roleDto.LocationId))
                throw new HttpException("Invalid Location");

            await RemoveTrackRole(roleDto);
        }

        private async Task RemoveTrackRole(TrackRoleDto roleDto)
        {
            if (!IsTrackRole(roleDto.Role))
                throw new HttpException($"Invalid role - {roleDto.Role}");

            var user = await DataContext.Users.Where(u => u.Id == roleDto.UserId).FirstOrDefaultAsync();
            if (user == null)
                throw new HttpException("User not found");

            var agent = await DataContext.TrackAgents
                .Where(a => a.UserId == roleDto.UserId && a.LocationId == roleDto.LocationId && a.Role == roleDto.Role)
                .FirstOrDefaultAsync();

            if (agent == null)
                throw new HttpException("User is not in this role");

            DataContext.TrackAgents.Remove(agent);

            if (!await SaveChanges())
                throw new HttpException("Failed to remove role");

            if (!await DataContext.TrackAgents.AnyAsync(a => a.UserId == roleDto.UserId && a.Role == roleDto.Role))
                await _userManager.RemoveFromRoleAsync(user, roleDto.Role);
        }

        public async Task RemoveRoleByStoreAdmin(int userId, StoreRoleDto roleDto)
        {
            if (!await IsStoreAdmin(userId, roleDto.StoreId))
                throw new HttpException("You are not StoreAdmin of this store.", StatusCodes.Status403Forbidden);

            await RemoveStoreRole(roleDto);
        }

        public async Task RemoveStoreRoleByModerator(int userId, StoreRoleDto roleDto)
        {
            if (!await IsStoreModerator(userId))
                throw new HttpException("You are not Moderator.", StatusCodes.Status403Forbidden);

            if (!await DataContext.Stores.AnyAsync(s => s.Id == roleDto.StoreId))
                throw new HttpException("Invalid Store");

            await RemoveStoreRole(roleDto);
        }

        private async Task RemoveStoreRole(StoreRoleDto roleDto)
        {
            if (!IsStoreRole(roleDto.Role))
                throw new HttpException($"Invalid role - {roleDto.Role}");

            var user = await DataContext.Users.Where(u => u.Id == roleDto.UserId).FirstOrDefaultAsync();
            if (user == null)
                throw new HttpException("User not found");

            var agent = await DataContext.StoresAgents
                .Where(a => a.UserId == roleDto.UserId && a.StoreId == roleDto.StoreId && a.Role == roleDto.Role)
                .FirstOrDefaultAsync();
            if (agent == null)
                throw new HttpException("User is not in this role");

            DataContext.StoresAgents.Remove(agent);

            if (!await SaveChanges())
                throw new HttpException("Failed to remove role");

            if (!await DataContext.StoresAgents.AnyAsync(a => a.UserId == roleDto.UserId && a.Role == roleDto.Role))
                await _userManager.RemoveFromRoleAsync(user, roleDto.Role);
        }

        public async Task<List<LocationInfoDto>> SearchLocations(int userId, LocationSearchParams searchParams)
        {
            IQueryable<Location> query;
            if (searchParams.For == RoleType.TrackModerator.ToString() || searchParams.For == RoleType.Admin.ToString())
                query = DataContext.Locations;
            else
                query = from agent in DataContext.TrackAgents
                        where agent.UserId == userId && agent.Role == RoleType.TrackAdmin.ToString()
                        join location in DataContext.Locations on agent.LocationId equals location.Id
                        select location;

            return await query.Where(l => l.Name.ToLower().Contains(searchParams.Name.ToLower()) && l.Type == searchParams.Type)
                .Take(16)
                .ProjectTo<LocationInfoDto>(Mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<StoreInfoDto>> SearchStores(int userId, StoreSearchParams searchParams)
        {
            IQueryable<Store> query;
            if (searchParams.For == RoleType.StoreModerator.ToString() || searchParams.For == RoleType.Admin.ToString())
                query = DataContext.Stores;
            else
                query = from agent in DataContext.StoresAgents
                        where agent.UserId == userId && agent.Role == RoleType.StoreAdmin.ToString()
                        join store in DataContext.Stores on agent.StoreId equals store.Id
                        select store;

            return await query.Where(s => s.Name.ToLower().Contains(searchParams.Name.ToLower()))
                .Take(16)
                .ProjectTo<StoreInfoDto>(Mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
