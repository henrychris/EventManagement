using ErrorOr;
using Shared.EventModels;

namespace EventModule.Interfaces;

public interface IEventService
{
    Task<EventResponse> CreateEvent(CreateEventRequest request);
    Task<ErrorOr<EventResponse>> GetEvent(string id);
    Task<EventResponse?> UpdateEvent(string id, UpdateEventRequest request);
    Task<bool> DeleteEvent(string id);
}