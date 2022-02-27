using API.DTOs;
using API.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Data;

public interface IRoleRepository
{
    Task<Response<StoreAgentDto, StoreRoleParams>> GetStoreAgentsForStoreAdmin(int userId, StoreRoleParams roleParams);
    Task<Response<StoreAgentDto, StoreRoleParams>> GetStoreAgentsForModerator(int userId, StoreRoleParams roleParams);
    Task<Response<TrackAgentDto, TrackRoleParams>> GetTrackAgentsForTrackAdmin(int userId, TrackRoleParams roleParams);
    Task<Response<TrackAgentDto, TrackRoleParams>> GetTrackAgentsFoModerator(int userId, TrackRoleParams roleParams);
    Task<Response<BaseAgentDto, BaseRoleParams>> GetModeratorsForAdmin(int userId, BaseRoleParams roleParams);
    Task<TrackAgentDto> AddRoleByTrackAdmin(int userId, TrackRoleDto roleDto);
    Task<TrackAgentDto> AddTrackRoleByModerator(int userId, TrackRoleDto roleDto);
    Task<StoreAgentDto> AddRoleByStoreAdmin(int userId, StoreRoleDto roleDto);
    Task<StoreAgentDto> AddStoreRoleByModerator(int userId, StoreRoleDto roleDto);
    Task<BaseAgentDto> AddModeratorByAdmin(int userId, BaseRoleDto roleDto);
    Task RemoveRoleByTrackAdmin(int userId, TrackRoleDto roleDto);
    Task RemoveTrackRoleByModerator(int userId, TrackRoleDto roleDto);
    Task RemoveRoleByStoreAdmin(int userId, StoreRoleDto roleDto);
    Task RemoveStoreRoleByModerator(int userId, StoreRoleDto roleDto);
    Task RemoveModeratorByAdmin(int userId, BaseRoleDto roleDto);
    Task<List<LocationInfoDto>> SearchLocations(int userId, LocationSearchParams searchParams);
    Task<List<StoreInfoDto>> SearchStores(int userId, StoreSearchParams searchParams);
}