using EventModule.Extensions;

namespace EventModule;

public static class EventModule
{
    public static void AddEventModule(this IServiceCollection services)
    {
        services.AddCore();
    }

    public static void UseEventModule(this IApplicationBuilder app)
    {
    }
}