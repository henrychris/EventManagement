namespace Shared.EventModels.Requests;

public record SearchEventRequest(string? EventName, DateTime? StartDate, DateTime? EndDate, decimal? MaxPrice, decimal MinPrice = 0.00m,
    int PageSize = SearchConstants.PageSize, int PageNumber = SearchConstants.PageNumber);