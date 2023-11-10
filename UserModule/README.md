# UserModule
The UserModule is a class library that provides functionality for handling events in a modular way.

## Usage
To use the UserModule, first add a reference to it from the API project. 

Next, you can add it to your IServiceCollection and IApplicationBuilder in your Startup.cs file:

```
using UserModule.Extensions;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddUserModule();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseUserModule();
    }
}
```

Once added, all services will be registered and controllers will be automatically detected by the framework.