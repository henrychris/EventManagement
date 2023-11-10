# EventModule
The EventModule is a class library that provides functionality for handling events in a modular way.

## Usage
To use the EventModule, first add a reference to it from the API project.  
Next, you can add it to your IServiceCollection and IApplicationBuilder in your Startup.cs file:

```
using EventModule.Extensions;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEventModule();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseEventModule();
    }
}
```

Once added, all services will be registered and controllers will be automatically detected by the framework. 