using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserModule.Data;
using UserModule.Data.Models;
using UserModule.Interfaces;
using UserModule.Repositories;
using UserModule.Services;
using UserModule.Validators;

namespace UserModule.Extensions;

public static class UserServiceExtensions
{
    internal static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddDatabase();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddIdentityServices();
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>(ServiceLifetime.Transient);
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        services.AddDbContext<UserDbContext>(options =>
            options.UseSqlite(config["ConnectionStrings:UserConnection"]));
        return services;
    }

    private static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}