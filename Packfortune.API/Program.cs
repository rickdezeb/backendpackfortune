using System;
using Packfortune.data;
using Packfortune.Logic;
using Packfortune.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using AspNetCoreRateLimit;
using AngleSharp;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Features;
using Packfortune.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Packfortune",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(connectionString);

builder.Services.AddDbContext<ApplicationDBContext>(options =>
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddScoped<IUserCoins, UserCoins>();
builder.Services.AddScoped<ICrates, Crates>();

builder.Services.AddScoped<UserCoinService>();
builder.Services.AddScoped<CratesService>();

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



builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 209715200;
});

// Add services to the container.

builder.Services.AddControllers();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseStaticFiles();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();


app.UseCors("Packfortune");

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();
