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
    /// <summary>
    /// Adds core services for the user module.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    internal static void AddCore(this IServiceCollection services)
    {
        AddDatabase(services);
        AddMSIdentity(services);
        RegisterCustomDependencies(services);
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>(ServiceLifetime.Transient);
    }

    /// <summary>
    /// Registers custom dependencies to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to register the dependencies to.</param>
    private static void RegisterCustomDependencies(IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();
    }

    /// <summary>
    /// Registers the database and unit of work used in the application.
    /// </summary>
    /// <param name="services">The IServiceCollection.</param>
    private static void AddDatabase(IServiceCollection services)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        services.AddDbContext<UserDbContext>(options =>
            options.UseSqlite(config["ConnectionStrings:UserConnection"]));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddMSIdentity(IServiceCollection services)
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
    }
}