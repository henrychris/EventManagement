using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Shared.UserModels.Responses;
using UserModule.Data.Models;
using UserModule.Interfaces;
using UserModule.ServiceErrors;

namespace UserModule.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ErrorOr<UserResponse>> GetUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Errors.User.NotFound;
        }

        return new UserResponse(user.Id, user.FirstName, user.LastName, user.Email!, user.WalletBalance, user.Role);
    }
}