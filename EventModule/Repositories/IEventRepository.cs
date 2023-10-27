using EventModule.Data.Models;
using Shared.EventModels.Requests;
using Shared.Repositories.Interfaces;

namespace EventModule.Repositories;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<IEnumerable<Event>> GetEventsWithAvailableTickets(int pageNumber, int pageSize);
    Task<IEnumerable<Event>> SearchEvents(SearchEventRequest request);
}