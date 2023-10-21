using ErrorOr;
using Shared.UserModels.Responses;

namespace UserModule.Interfaces;

public interface IUserService
{
    Task<ErrorOr<UserResponse>> GetUser(string userId);
}