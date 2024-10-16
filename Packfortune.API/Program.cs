using System;
using Packfortune.data;
using Packfortune.Logic;
using Packfortune.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using AspNetCoreRateLimit;
using AngleSharp;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;



var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(connectionString);

builder.Services.AddDbContext<ApplicationDBContext>(options =>
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddScoped<IAddUserCoins, AddUserCoins>();

builder.Services.AddScoped<UserCoinService>();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Test", options =>
    {
        options.AutoReplenishment = true;
        options.PermitLimit = 10;
        options.Window = TimeSpan.FromMinutes(1);
    });

    options.OnRejected = async (context, token) =>
    {
        Console.WriteLine("Rate limit exceeded.");
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync(
            "Too many requests. Please try again later.", cancellationToken: token);
    };
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();
