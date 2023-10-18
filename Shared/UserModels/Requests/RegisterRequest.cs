using System.ComponentModel.DataAnnotations;

namespace Shared.UserModels.Requests;

public record RegisterRequest
{
    public RegisterRequest(string firstName, string lastName, string emailAddress, string password, string role)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        Password = password;
        Role = role;
    }

    public string FirstName { get; init; }
    public string LastName { get; init; }

    [DataType(DataType.EmailAddress)] public string EmailAddress { get; init; }

    public string Password { get; init; }
    public string Role { get; init; }
}