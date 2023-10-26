using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.UserModels;
using UserModule.Services;

namespace UserModule.Tests.Services;

[TestFixture]
public class TokenServiceTests
{
    private IConfiguration _configuration;
    private TokenService _tokenService;

    [SetUp]
    public void Setup()
    {
        var base64Key = GetRandomSigningKey();

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("JwtSettings:Key", base64Key),
                new KeyValuePair<string, string>("JwtSettings:Audience", "my_audience"),
                new KeyValuePair<string, string>("JwtSettings:Issuer", "my_issuer"),
                new KeyValuePair<string, string>("JwtSettings:TokenLifetimeInHours", "1")
            }!)
            .Build();

        _tokenService = new TokenService(_configuration);
    }

    private static string GetRandomSigningKey()
    {
        var keyBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }

        var base64Key = Convert.ToBase64String(keyBytes);
        return base64Key;
    }

    [Test]
    public void CreateUserJwt_Should_Return_Valid_Token()
    {
        // Arrange
        const string emailAddress = "test@example.com";
        const string userRole = "admin";
        const string userId = "123";

        // Act
        var token = _tokenService.CreateUserJwt(emailAddress, userRole, userId);

        // Assert
        token.Should().NotBeNull();

        var tokenHandler = new JwtSecurityTokenHandler();
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidIssuer = _configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = _configuration["JwtSettings:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
        claimsPrincipal.Should().NotBeNull();

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        jwtSecurityToken.Should().NotBeNull();

        emailAddress.Should().BeEquivalentTo(jwtSecurityToken?.Payload[JwtClaims.Email] as string);
        userId.Should().BeEquivalentTo(jwtSecurityToken?.Payload[JwtClaims.UserId] as string);
        userRole.Should().BeEquivalentTo(jwtSecurityToken?.Payload[JwtClaims.Role] as string);

        // Extract expiration time from the token
        var expiration = jwtSecurityToken?.ValidTo;

        // Calculate the expected expiration time (current time + token lifetime)
        var expectedExpiration = DateTime.UtcNow.AddHours(1);

        // Assert that the token's expiration matches the expected expiration
        expectedExpiration.Should().BeCloseTo(expiration.GetValueOrDefault(), TimeSpan.FromSeconds(10));
    }
}