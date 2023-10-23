﻿using EventModule.Data;
using EventModule.Interfaces;
using EventModule.Repositories;
using EventModule.Services;
using EventModule.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EventModule.Extensions;

public static class EventServiceExtensions
{
    internal static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();
        services.AddAutoMapper(typeof(EventMappingProfile));
        services.AddValidatorsFromAssemblyContaining<CreateEventRequestValidator>(ServiceLifetime.Transient);
        services.AddDatabase();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
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