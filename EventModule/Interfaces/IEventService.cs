using EventModule.Data.Models;
using Shared.EventModels;

namespace EventModule.Interfaces;

public interface IEventService
{
    Task<EventResponse> CreateEvent(CreateEventRequest request);
    Task<EventResponse?> GetEvent(string id);
    Task<EventResponse?> UpdateEvent(string id, UpdateEventRequest request);
    Task<bool> DeleteEvent(string id);
}