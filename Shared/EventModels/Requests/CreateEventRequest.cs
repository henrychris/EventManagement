namespace Shared.EventModels.Requests;

public record CreateEventRequest(string Name, string Description, decimal Price, DateTime Date, DateTime StartTime,
    DateTime EndTime);
    