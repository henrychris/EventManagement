namespace Shared.UserModels.Responses;

public record UserAuthResponse(string Id,
    string FirstName,
    string LastName,
    string EmailAddress,
    decimal WalletBalance,
    string Role,
    string AccessToken);