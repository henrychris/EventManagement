using EventModule;
using Shared.API;
using UserModule;

namespace API;

public static class MiddlewareInitializer
{
    /// <summary>
    /// Configures middleware for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void ConfigureMiddleware(this WebApplication app)
    {
        RegisterSwagger(app);
        RegisterMiddleware(app);
        RegisterModules(app);
    }

    /// <summary>
    /// Registers Swagger middleware for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    private static void RegisterSwagger(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    /// <summary>
    /// Registers middleware for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    private static void RegisterMiddleware(WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseMiddleware<ExceptionMiddleware>();
    }

    /// <summary>
    /// Registers modules for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    private static void RegisterModules(IApplicationBuilder app)
    {
        app.UseEventModule();
        app.UseUserModule();
    }
}