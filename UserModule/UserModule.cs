using UserModule.Extensions;

namespace UserModule;

public static class UserModule
{
    public static IServiceCollection AddUserModule(this IServiceCollection services)
    {
        services.AddCore();
        return services;
    }

    public static IApplicationBuilder UseUserModule(this IApplicationBuilder app)
    {
        return app;
    }
}