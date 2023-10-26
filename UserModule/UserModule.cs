using UserModule.Extensions;

namespace UserModule;

public static class UserModule
{
    public static void AddUserModule(this IServiceCollection services)
    {
        services.AddCore();
    }

    public static void UseUserModule(this IApplicationBuilder app)
    {
    }
}