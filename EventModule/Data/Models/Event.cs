using Shared.Enums;

namespace EventModule.Data.Models;

public class Event
{
    public const int MinNameLength  = 3;
    public const int MaxNameLength  = 30;
    
    public const int MinDescriptionLength = 3;
    public const int MaxDescriptionLength = 100;
    
    // Properties
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime Date { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
    public EventStatus EventStatus { get; set; } = EventStatus.Upcoming;
}