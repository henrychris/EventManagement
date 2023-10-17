using EventModule.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EventModule.Data;

public class EventDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public EventDbContext(IConfiguration configuration)
    {
        // todo: use configuration records to store settings
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sql server database
        options.UseSqlite(_configuration.GetConnectionString("DefaultConnection"));
    }

    public DbSet<Event> Events { get; set; } = null!;
}