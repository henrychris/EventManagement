using EventModule;
using EventModule.Data;
using EventModule.Interfaces;
using EventModule.Services;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddAutoMapper(typeof(EventMappingProfile));


var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();
// todo: move DB and Services setup into separate files.
builder.Services.AddDbContext<EventDbContext>(options => options.UseSqlite());
using var context = new EventDbContext(config);
context.Database.EnsureCreated();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();