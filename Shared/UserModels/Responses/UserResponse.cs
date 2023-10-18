namespace Shared.UserModels.Responses;

public record UserResponse(string Id,
    string FirstName,
    string LastName,
    string EmailAddress,
    decimal WalletBalance,
    string Role,
    string AccessToken);