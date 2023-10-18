using EventModule.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EventModule.Data;

public class EventDbContext : DbContext
{
    public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events { get; set; } = null!;
}