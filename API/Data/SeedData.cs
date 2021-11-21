using API.Entities;
using API.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace API.Data
{
    public class SeedData
    {
        public static async Task SeedRoles(RoleManager<IdentityRole<int>> roleManager)
        {
            if (await roleManager.Roles.AnyAsync()) return;

            foreach (var role in Enum.GetNames<Role>())
            {
                await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
            }
        }
    }
}
