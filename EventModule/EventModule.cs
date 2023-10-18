using EventModule.Extensions;

namespace EventModule;

public static class EventModule
{
    public static IServiceCollection AddEventModule(this IServiceCollection services)
    {
        services.AddCore();
        return services;
    }

    public static IApplicationBuilder UseEventModule(this IApplicationBuilder app)
    {
        return app;
    }
}