namespace Shared.EventModels.Responses;

public record SearchEventResponse(List<EventResponse> Events, int Count);