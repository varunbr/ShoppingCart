using API.Extensions;
using API.Seed;
using API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});


//middleware
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(options =>
options.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://varunbr.github.io", "http://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapFallbackToController("Index", "Fallback");

//Seed
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var seed = services.GetService<SeedData>();
    await seed.SeedDatabase();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

//Run application
app.Run();