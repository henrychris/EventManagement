using Shared.Enums;

namespace Shared.EventModels.Requests;

public record SearchEventRequest(string? EventName, DateTime? StartDate, DateTime? EndDate, decimal? MaxPrice,
    decimal MinPrice = 0.00m) : SearchRequestBase(SearchConstants.PageSize, SearchConstants.PageNumber,
    EventSortStrings.DateAsc);