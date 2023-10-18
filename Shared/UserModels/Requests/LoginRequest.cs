using System.ComponentModel.DataAnnotations;

namespace Shared.UserModels.Requests;

public record LoginRequest
{
    public LoginRequest(string emailAddress, string password)
    {
        EmailAddress = emailAddress;
        Password = password;
    }

    [DataType(DataType.EmailAddress)]
    public string EmailAddress { get; init; }

    [DataType(DataType.Password)] public string Password { get; init; }
}