using EventModule.Data;
using EventModule.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace EventModule.Extensions;

public static class EventAppExtensions
{
    private const string DefaultEventName = "Henry's Main Event!";
    private const string DefaultEventId = "cecb7257-6764-4a5c-a9f8-6412d158214a";
    private const string InMemoryProviderName = "Microsoft.EntityFrameworkCore.InMemory";

    public static async Task SeedDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EventDbContext>();

        Console.WriteLine("EventModule: database seeding starting.");
        if (IsInMemoryDatabase(context))
        {
            await SeedInMemoryDatabase(context);
        }
        else
        {
            await MigrateAndSeedDevelopmentDatabase(context);
        }

        await context.SaveChangesAsync();
        Console.WriteLine("EventModule: database seeding complete.");
    }

    private static async Task SeedInMemoryDatabase(EventDbContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    private static async Task MigrateAndSeedDevelopmentDatabase(EventDbContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        await SeedEvents(context);
    }

    private static async Task SeedEvents(EventDbContext context)
    {
        if (!await context.Events.AnyAsync(x => x.Name == DefaultEventName))
        {
            await context.Events.AddAsync(new Event
            {
                Id = DefaultEventId,
                Name = DefaultEventName,
                Description = "An event that exists by default.",
                Price = 0.00m,
                EventStatus = EventStatus.Upcoming,
                Date = DateTime.Now.AddDays(1).Date,
                StartTime = DateTime.Now.AddDays(1).AddHours(1).Date,
                EndTime = DateTime.Now.AddDays(1).AddHours(6).Date
            });

            Console.WriteLine($"EventModule: Added default event: {DefaultEventName}");
        }

        if (!await context.Events.AnyAsync(x => x.Name == "Summer Music Concert"))
        {
            var date = new DateTime(2023, 7, 29);
            await context.Events.AddAsync(new Event
            {
                Id = "61c904c7-fdb2-4f56-859e-e6a5151af515",
                Name = "Summer Music Concert",
                Description = "An event that exists by default.",
                Price = 200.00m,
                EventStatus = EventStatus.Canceled,
                Date = date,
                StartTime = date.AddHours(1).Date,
                EndTime = date.AddHours(6).Date
            });
        }
    }

    private static bool IsInMemoryDatabase(DbContext context)
    {
        return context.Database.ProviderName == InMemoryProviderName;
    }
}