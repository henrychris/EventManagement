using API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterApplicationServices();

var app = builder.Build();
app.ConfigureMiddleware();

app.Run();