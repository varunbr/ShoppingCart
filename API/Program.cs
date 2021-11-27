using API.Data;
using API.Entities;
using API.Seed;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();
                var roleManager = services.GetService<RoleManager<IdentityRole<int>>>();
                var userManager = services.GetService<UserManager<User>>();
                var config = services.GetRequiredService<IConfiguration>();
                var mapper = services.GetService<IMapper>();
                await context.Database.MigrateAsync();
                await SeedData.SeedRoles(roleManager);
                await SeedData.SeedLocation(context, mapper);
                await SeedData.SeedUsers(userManager, context, config);
                await SeedData.SeedStore(context);
                await SeedData.SeedCategory(context, mapper);
                await SeedData.SeedProduct(context, mapper);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during migration");
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
