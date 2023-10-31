using EventModule.Data;
using EventModule.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Enums;
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

    public async Task<IEnumerable<Event>> GetEventsWithAvailableTickets(int pageNumber, int pageSize,
        EventSortOption sortOption)
    {
        pageNumber = pageNumber <= 0 ? SearchConstants.PageNumber : pageNumber;
        pageSize = pageSize < 0 ? SearchConstants.PageSize : pageSize;

        var query = SortResults(EventDbContext.Events
            .Where(x => x.TicketsAvailable > 0)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize), sortOption);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Event>> SearchEvents(SearchEventRequest request, EventSortOption sort)
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

        query = SortResults(query, sort);
        return await query
            .Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .ToListAsync();
    }

    private static IQueryable<Event> SortResults(IQueryable<Event> query, EventSortOption sort)
    {
        query = sort switch
        {
            EventSortOption.NameAsc => query.OrderBy(x => x.Name),
            EventSortOption.NameDesc => query.OrderByDescending(x => x.Name),
            EventSortOption.DateAsc => query.OrderBy(x => x.Date),
            EventSortOption.DateDesc => query.OrderByDescending(x => x.Date),
            EventSortOption.PriceAsc => query.OrderBy(x => x.Price),
            EventSortOption.PriceDesc => query.OrderByDescending(x => x.Price),
            EventSortOption.TicketsAvailableAsc => query.OrderBy(x => x.TicketsAvailable),
            EventSortOption.TicketsAvailableDesc => query.OrderByDescending(x => x.TicketsAvailable),
            _ => query.OrderBy(x => x.Date)
        };

        return query;
    }
}