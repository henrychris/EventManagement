# EventModule
The EventModule is a C# library that provides functionality for handling events in a modular way.

## Installation
To use the EventModule in your project, you can install it via NuGet:

## Usage
To use the EventModule, you can add it to your IServiceCollection and IApplicationBuilder in your Startup.cs file:

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
Once the EventModule is added to your project, you can use it to handle events in a modular way.