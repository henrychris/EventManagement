using Shared.Enums;

namespace Shared.EventModels.Responses;

public record EventResponse(string Guid,
    string Name,
    string Description,
    decimal Price,
    DateTime Date,
    DateTime StartTime,
    DateTime EndTime, EventStatus EventStatus,
    int TicketsSold, int TicketsAvailable);