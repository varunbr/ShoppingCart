using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config, UserManager<User> userManager)
        {
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public async Task<string> CreateToken(User user, string existingToken = null)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName,user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(user);

            var expireDate = string.IsNullOrEmpty(existingToken)
                ? DateTime.Now.AddDays(1)
                : GetExpireDate(existingToken);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = cred,
                Subject = new ClaimsIdentity(claims),
                Expires = expireDate
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private DateTime GetExpireDate(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return tokenHandler.ValidTo;
        }
    }
}
