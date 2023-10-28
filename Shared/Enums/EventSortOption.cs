namespace Shared.Enums;

public enum EventSortOption
{
    DateAsc, // default
    NameAsc,
    NameDesc,
    DateDesc,
    PriceAsc,
    PriceDesc,
    TicketsAvailableAsc,
    TicketsAvailableDesc
}

public static class EventSortStrings
{
    public const string DateAsc = nameof(DateAsc);
    // public const string NameAsc = nameof(NameAsc);
    // public const string NameDesc = nameof(NameDesc);
    // public const string DateDesc = nameof(DateDesc);
    // public const string PriceAsc = nameof(PriceAsc);
    // public const string PriceDesc = nameof(PriceDesc);
}