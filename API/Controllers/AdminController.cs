using API.Data;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        private readonly IUnitOfWork _uow;
        public AdminController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("moderate/admin-role")]
        public async Task<ActionResult> GetModeratorsForAdmin([FromQuery] BaseRoleParams roleParams)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.RoleRepository.GetModeratorsForAdmin(userId, roleParams));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("moderate/admin-role")]
        public async Task<ActionResult> AddModeratorByAdmin(BaseRoleDto roleDto)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.RoleRepository.AddModeratorByAdmin(userId, roleDto));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("moderate/admin-role")]
        public async Task<ActionResult> RemoveModeratorByAdmin(BaseRoleDto roleDto)
        {
            var userId = HttpContext.User.GetUserId();

            if (roleDto.UserId == userId)
                return BadRequest("You cannot remove your role");

            await _uow.RoleRepository.RemoveModeratorByAdmin(userId, roleDto);
            return Ok();
        }

        [Authorize(Roles = "TrackModerator,Admin")]
        [HttpGet("moderate/track-role")]
        public async Task<ActionResult> GetTrackAgentsForModerator([FromQuery] TrackRoleParams roleParams)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.RoleRepository.GetTrackAgentsFoModerator(userId, roleParams));
        }

        [Authorize(Roles = "TrackModerator,Admin")]
        [HttpPost("moderate/track-role")]
        public async Task<ActionResult> AddTrackRoleByModerator(TrackRoleDto roleDto)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.RoleRepository.AddTrackRoleByModerator(userId, roleDto));
        }

        [Authorize(Roles = "TrackModerator,Admin")]
        [HttpDelete("moderate/track-role")]
        public async Task<ActionResult> RemoveTrackRoleByModerator(TrackRoleDto roleDto)
        {
            var userId = HttpContext.User.GetUserId();
            await _uow.RoleRepository.RemoveTrackRoleByModerator(userId, roleDto);
            return Ok();
        }

        [Authorize(Roles = "StoreModerator,Admin")]
        [HttpGet("moderate/store-role")]
        public async Task<ActionResult> GetStoreAgentsForModerator([FromQuery] StoreRoleParams roleParams)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.RoleRepository.GetStoreAgentsForModerator(userId, roleParams));
        }

        [Authorize(Roles = "StoreModerator,Admin")]
        [HttpPost("moderate/store-role")]
        public async Task<ActionResult> AddStoreRoleByModerator(StoreRoleDto roleDto)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.RoleRepository.AddStoreRoleByModerator(userId, roleDto));
        }

        [Authorize(Roles = "StoreModerator,Admin")]
        [HttpDelete("moderate/store-role")]
        public async Task<ActionResult> RemoveStoreRoleByModerator(StoreRoleDto roleDto)
        {
            var userId = HttpContext.User.GetUserId();
            await _uow.RoleRepository.RemoveStoreRoleByModerator(userId, roleDto);
            return Ok();
        }

        [Authorize(Roles = "TrackAdmin")]
        [HttpGet("track-role")]
        public async Task<ActionResult> GetTrackAgentsForTrackAdmin([FromQuery] TrackRoleParams roleParams)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.RoleRepository.GetTrackAgentsForTrackAdmin(userId, roleParams));
        }

        [Authorize(Roles = "TrackAdmin")]
        [HttpPost("track-role")]
        public async Task<ActionResult> AddRoleByTrackAdmin(TrackRoleDto roleDto)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.RoleRepository.AddRoleByTrackAdmin(userId, roleDto));
        }

        [Authorize(Roles = "TrackAdmin")]
        [HttpDelete("track-role")]
        public async Task<ActionResult> RemoveRoleByTrackAdmin(TrackRoleDto roleDto)
        {
            var userId = HttpContext.User.GetUserId();

            if (roleDto.UserId == userId)
                return BadRequest("You cannot remove your role");

            await _uow.RoleRepository.RemoveRoleByTrackAdmin(userId, roleDto);
            return Ok();
        }

        [Authorize(Roles = "StoreAdmin")]
        [HttpGet("store-role")]
        public async Task<ActionResult> GetStoreAgentsForStoreAdmin([FromQuery] StoreRoleParams roleParams)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.RoleRepository.GetStoreAgentsForStoreAdmin(userId, roleParams));
        }

        [Authorize(Roles = "StoreAdmin")]
        [HttpPost("store-role")]
        public async Task<ActionResult> AddRoleByStoreAdmin(StoreRoleDto roleDto)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.RoleRepository.AddRoleByStoreAdmin(userId, roleDto));
        }

        [Authorize(Roles = "StoreAdmin")]
        [HttpDelete("store-role")]
        public async Task<ActionResult> RemoveRoleByStoreAdmin(StoreRoleDto roleDto)
        {
            var userId = HttpContext.User.GetUserId();

            if (roleDto.UserId == userId)
                return BadRequest("You cannot remove your role");

            await _uow.RoleRepository.RemoveRoleByStoreAdmin(userId, roleDto);
            return Ok();
        }

        [HttpGet("search-locations")]
        public async Task<ActionResult> SearchLocations([FromQuery] LocationSearchParams searchParams)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.RoleRepository.SearchLocations(userId, searchParams));
        }

        [HttpGet("search-stores")]
        public async Task<ActionResult> SearchStores([FromQuery] StoreSearchParams searchParams)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.RoleRepository.SearchStores(userId, searchParams));
        }
    }
}
