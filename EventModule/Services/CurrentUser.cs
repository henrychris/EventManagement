using System.Security.Claims;
using EventModule.Interfaces;
using Shared.UserModels;

namespace EventModule.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId =>
        // Retrieve the user ID from the JWT token
        _httpContextAccessor.HttpContext?.User.FindFirst(JwtClaims.UserId)?.Value;

    public string? Email =>
        // Retrieve the email from the JWT token
        _httpContextAccessor.HttpContext?.User.FindFirst(JwtClaims.Email)?.Value;

    public string? Role =>
        // Retrieve the role from the JWT token
        _httpContextAccessor.HttpContext?.User.FindFirst(JwtClaims.Role)?.Value;
}