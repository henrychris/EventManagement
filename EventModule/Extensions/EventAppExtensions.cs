using EventModule.Data;
using EventModule.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace EventModule.Extensions;

public static class EventAppExtensions
{
    public static async Task SeedDatabase(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            Console.WriteLine("Starting EventModule database seeding.");
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<EventDbContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            if (!await context.Events.AnyAsync(x => x.Name == "Henry's Main Event!"))
            {
                await context.Events.AddAsync(new Event
                {
                    Id = "cecb7257-6764-4a5c-a9f8-6412d158214a",
                    Name = "Henry's Main Event!",
                    Description = "An event that exists by default.",
                    Price = 0.00m,
                    EventStatus = EventStatus.Upcoming,
                    Date = DateTime.Now.AddDays(1).Date,
                    StartTime = DateTime.Now.AddDays(1).AddHours(1).Date,
                    EndTime = DateTime.Now.AddDays(1).AddHours(6).Date
                });
            }

            await context.SaveChangesAsync();
            Console.WriteLine("EventModule database seeding complete.");
        }
    }
}