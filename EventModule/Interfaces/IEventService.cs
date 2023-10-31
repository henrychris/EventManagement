using ErrorOr;
using Shared.EventModels.Requests;
using Shared.EventModels.Responses;

namespace EventModule.Interfaces;

public interface IEventService
{
    Task<ErrorOr<EventResponse>> CreateEvent(CreateEventRequest request);
    Task<ErrorOr<EventResponse>> GetEvent(string id);
    Task<ErrorOr<EventResponse>> UpdateEvent(string id, UpdateEventRequest request);
    Task<ErrorOr<Deleted>> DeleteEvent(string id);
    Task<ErrorOr<SearchEventResponse>> SearchEvents(SearchEventRequest request);
    Task<ErrorOr<SearchEventResponse>> GetEventsWithAvailableTickets(int pageNumber, int pageSize, string sort);

    Task<ErrorOr<TicketPurchaseResponse>> BuyTicket(string id);
}