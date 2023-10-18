using EventModule.Data;
using EventModule.Interfaces;
using EventModule.Services;
using Microsoft.EntityFrameworkCore;

namespace EventModule.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();
        services.AddAutoMapper(typeof(EventMappingProfile));
        services.AddDatabase();
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        services.AddDbContext<EventDbContext>(options =>
            options.UseSqlite(config["ConnectionStrings:EventConnection"]));
        return services;
    }
}