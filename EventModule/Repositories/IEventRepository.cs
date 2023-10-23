using EventModule.Data.Models;
using Shared.Repositories.Interfaces;

namespace EventModule.Repositories;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<IEnumerable<Event>> GetEventsWithAvailableTickets();
}