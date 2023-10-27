using EventModule.Data;
using EventModule.Interfaces;
using EventModule.Repositories;
using EventModule.Services;
using EventModule.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EventModule.Extensions;

public static class EventServiceExtensions
{
    /// <summary>
    /// Adds core services for the event module.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    internal static void AddCore(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(EventMappingProfile));
        services.AddValidatorsFromAssemblyContaining<CreateEventRequestValidator>(ServiceLifetime.Transient);

        AddDatabase(services);
        RegisterCustomDependencies(services);
    }

    /// <summary>
    /// Registers custom dependencies to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to register the dependencies to.</param>
    private static void RegisterCustomDependencies(IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();
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
        services.AddDbContext<EventDbContext>(options =>
            options.UseSqlServer(config["ConnectionStrings:EventConnection"]));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}