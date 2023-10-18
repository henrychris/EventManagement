using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shared.UserModels;
using UserModule.Interfaces;

namespace UserModule.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;

    public AuthenticationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateUserJwt(string emailAddress, string userRole, string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"] ??
                                   throw new InvalidOperationException("Security Key is null!")));

        var claims = new List<Claim>
        {
            new(JwtClaims.Email, emailAddress),
            new(JwtClaims.UserId, userId),
            new(JwtClaims.UserRole, userRole)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _configuration["JwtSettings:Audience"],
            Issuer = _configuration["JwtSettings:Issuer"],
            Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["JwtSettings:TokenLifetimeInHours"])),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}