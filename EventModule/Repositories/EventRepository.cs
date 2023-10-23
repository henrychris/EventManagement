using EventModule.Data;
using EventModule.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories.Implementations;

namespace EventModule.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(DbContext context) : base(context)
    {
    }

    private EventDbContext EventDbContext => Context as EventDbContext ??
                                             throw new InvalidCastException("Event DB Context not passed from unit of work.");

    public async Task<IEnumerable<Event>> GetEventsWithAvailableTickets()
    {
        return await EventDbContext.Events.ToListAsync();
    }
}