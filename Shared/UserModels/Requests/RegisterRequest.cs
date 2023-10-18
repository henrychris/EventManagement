using System.ComponentModel.DataAnnotations;

namespace Shared.UserModels.Requests;

public record RegisterRequest(
    [Required, StringLength(50)]
    string FirstName,
    [Required, StringLength(50)]
    string LastName,
    [Required, DataType(DataType.EmailAddress)]
    string EmailAddress,
    string Password,
    string Role);