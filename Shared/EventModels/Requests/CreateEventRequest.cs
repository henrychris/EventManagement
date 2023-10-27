namespace Shared.EventModels.Requests;

public record CreateEventRequest(string Name, string Description, DateTime Date, DateTime StartTime,
    DateTime EndTime, int TicketsAvailable = 1, decimal Price = 0.0m);
    