using System.Text;
using EventModule;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.Filters;
using Shared.UserModels;
using UserModule;

namespace API;

public static class ServiceInitializer
{
    /// <summary>
    /// Registers application services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        SetupControllers(services);
        RegisterModules(services);
        RegisterSwagger(services);
        SetupAuth(services);
        RegisterCustomDependencies(services);
    }

    /// <summary>
    /// Registers Swagger/OpenAPI services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    private static void RegisterSwagger(IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    /// <summary>
    /// Sets up authentication services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    private static void SetupAuth(IServiceCollection services)
    {
        // todo: this should be for dev environment only sha.
        // check how to bind the secrets to a class.
        var secrets = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(
                x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = secrets["JwtSettings:Audience"],
                        ValidIssuer = secrets["JwtSettings:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrets["JwtSettings:Key"] ??
                            throw new InvalidOperationException("Security Key is null!"))),
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RoleClaimType = JwtClaims.Role
                    };
                });
        services.AddAuthorization();
    }

    /// <summary>
    /// Registers application modules.
    /// </summary>
    /// <param name="services">The service collection.</param>
    private static void RegisterModules(IServiceCollection services)
    {
        services.AddEventModule();
        services.AddUserModule();
    }

    /// <summary>
    /// Sets up controller services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    private static void SetupControllers(IServiceCollection services)
    {
        services.AddControllers();
        services.AddRouting(options => options.LowercaseUrls = true);
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }

    private static void RegisterCustomDependencies(IServiceCollection services)
    {
        services.AddScoped<CustomValidationFilter>();
    }
}