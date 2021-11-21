using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            IUnitOfWork uow, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _uow = uow;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserRegisterDto registerDto)
        {
            if (!Enum.GetNames<Gender>().Contains(registerDto.Gender))
                return BadRequest("Invalid gender.");
            if (await UserNameExist(registerDto.UserName))
                return BadRequest("Username is taken.");
            if (await _userManager.Users.AnyAsync(u => u.NormalizedEmail == registerDto.Email.ToUpper()))
                return BadRequest("Email is already registered.");

            var user = _mapper.Map<User>(registerDto);
            user.LastActive = DateTime.UtcNow;
            user.Account = new Account { Balance = 1000 };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            result = await _userManager.AddToRoleAsync(user, Role.User.ToString());
            if (!result.Succeeded) return BadRequest(result.Errors);

            var token = await _tokenService.CreateToken(user);

            return new UserDto
            {
                Name = user.Name,
                UserName = user.UserName,
                Token = token,
                PhotoUrl = user.Photo?.Url
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(u => u.Photo)
                .SingleOrDefaultAsync(u => u.UserName == loginDto.UserName.ToLower());

            if (user == null) return BadRequest("Invalid user");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized();

            var token = await _tokenService.CreateToken(user);

            return new UserDto
            {
                Name = user.Name,
                UserName = user.UserName,
                Token = token,
                PhotoUrl = user.Photo?.Url
            };
        }

        [Authorize]
        [HttpPost("token-update")]
        public async Task<ActionResult<UserDto>> GetUpdatedToken()
        {
            var id = HttpContext.User.GetUserId();
            var user = await _userManager.Users
                .Include(u => u.Photo)
                .SingleAsync(u => u.Id == id);

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var token = await _tokenService.CreateToken(user, accessToken);

            return new UserDto
            {
                Name = user.Name,
                UserName = user.UserName,
                Token = token,
                PhotoUrl = user.Photo?.Url
            };
        }

        [HttpGet("{userName}")]
        public async Task<bool> UserNameExist(string userName)
        {
            return await _uow.UserRepository.UserExist(userName);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult> GetUserProfile()
        {
            var id = HttpContext.User.GetUserId();
            return Ok(await _uow.UserRepository.GetProfile(id));
        }

        [Authorize]
        [HttpPost("profile")]
        public async Task<ActionResult<UserProfileDto>> UpdateUserProfile(UserProfileDto profileDto)
        {
            profileDto.Id = HttpContext.User.GetUserId();

            if (!Enum.GetNames<Gender>().Contains(profileDto.Gender))
                return BadRequest("Invalid gender.");
            if (await _userManager.Users.AnyAsync(u => u.Id != profileDto.Id &&
                                                       u.NormalizedUserName == profileDto.UserName.ToUpper()))
                return BadRequest("Username is taken.");
            if (await _userManager.Users.AnyAsync(u => u.Id != profileDto.Id &&
                                                       u.NormalizedEmail == profileDto.Email.ToUpper()))
                return BadRequest("Email is already registered.");

            var user = await _userManager.Users.SingleAsync(u => u.Id == profileDto.Id);
            _mapper.Map(profileDto, user);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest("Failed to update.");

            _mapper.Map(user, profileDto);
            return profileDto;
        }
    }
}
