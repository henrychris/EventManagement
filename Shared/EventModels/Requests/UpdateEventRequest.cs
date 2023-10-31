namespace Shared.EventModels.Requests;

// note, for this one, the eventId should be passed as a query parameter
public record UpdateEventRequest(
    string? Name,
    string? Description,
    decimal? Price,
    DateTime? Date,
    DateTime? StartTime,
    DateTime? EndTime,
    string? EventStatus, int? TicketsAvailable);

// todo: update event capacity for admin. they can adjust the number of tickets sold.
