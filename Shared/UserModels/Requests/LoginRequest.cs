using System.ComponentModel.DataAnnotations;

namespace Shared.UserModels.Requests;

public record LoginRequest([EmailAddress] string EmailAddress, string Password);