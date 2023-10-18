using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;

namespace UserModule.Data.Models;

public class ApplicationUser : IdentityUser
{
    [Required, MaxLength(50)] public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(50)] public string LastName { get; set; } = string.Empty;
    public decimal WalletBalance { get; set; } = 0.00m;
    public string Role { get; set; } = UserRoles.User.ToString();
}