using ErrorOr;
using Shared.UserModels.Requests;
using Shared.UserModels.Responses;

namespace UserModule.Interfaces;

public interface IAuthenticationService
{
    Task<ErrorOr<UserAuthResponse>> RegisterAsync(RegisterRequest request);
    Task<ErrorOr<UserAuthResponse>> LoginAsync(LoginRequest request);
}