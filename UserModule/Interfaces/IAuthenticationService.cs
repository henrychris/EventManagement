using ErrorOr;
using Shared.UserModels.Requests;
using Shared.UserModels.Responses;

namespace UserModule.Interfaces;

public interface IAuthenticationService
{
    Task<ErrorOr<UserResponse>> RegisterAsync(RegisterRequest request);
    Task<ErrorOr<UserResponse>> LoginAsync(LoginRequest request);
}