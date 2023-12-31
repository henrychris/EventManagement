﻿using EventModule.Data.Models;
using Shared.Enums;
using Shared.EventModels.Requests;
using Shared.Repositories.Interfaces;

namespace EventModule.Repositories;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<IEnumerable<Event>> GetEventsWithAvailableTickets(int pageNumber, int pageSize, EventSortOption sortOption);
    Task<IEnumerable<Event>> SearchEvents(SearchEventRequest request, EventSortOption sort);
}