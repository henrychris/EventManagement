using ErrorOr;
using Shared.EventModels;

namespace EventModule.Interfaces;

public interface IEventService
{
    Task<ErrorOr<EventResponse>> CreateEvent(CreateEventRequest request);
    Task<ErrorOr<EventResponse>> GetEvent(string id);
    Task<ErrorOr<EventResponse>> UpdateEvent(string id, UpdateEventRequest request);
    Task<ErrorOr<Deleted>> DeleteEvent(string id);
}