using EventModule.Data;
using EventModule.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.EventModels.Requests;
using Shared.Repositories.Implementations;

namespace EventModule.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(DbContext context) : base(context)
    {
    }

    private EventDbContext EventDbContext => Context as EventDbContext ??
                                             throw new InvalidCastException(
                                                 "Event DB Context not passed from unit of work.");

    public async Task<IEnumerable<Event>> GetEventsWithAvailableTickets(int pageNumber, int pageSize)
    {
        pageNumber = pageNumber <= 0 ? SearchConstants.PageNumber : pageNumber;
        pageSize = pageSize < 0 ? SearchConstants.PageSize : pageSize;
        
        return await EventDbContext.Events
            .Where(x => x.TicketsAvailable > 0)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> SearchEvents(SearchEventRequest request)
    {
        var eventName = request.EventName ?? "";
        var minPrice = request.MinPrice < 0 ? 0 : request.MinPrice;
        var query = EventDbContext.Events.Where(x => x.Name.Contains(eventName) && x.Price >= minPrice);

        if (request.StartDate.HasValue)
        {
            query = query.Where(x => x.Date >= request.StartDate);
        }

        if (request.EndDate.HasValue)
        {
            query = query.Where(x => x.Date <= request.EndDate);
        }

        if (request.MaxPrice.HasValue)
        {
            var maxPrice = request.MaxPrice < 0 ? 0 : request.MaxPrice;
            query = query.Where(x => x.Price <= maxPrice);
        }

        return await query
            .Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .ToListAsync();
    }
}