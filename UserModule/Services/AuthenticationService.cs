using System.Text.Json;
using AutoMapper;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;
using Shared.UserModels;
using Shared.UserModels.Requests;
using Shared.UserModels.Responses;
using UserModule.Data.Models;
using UserModule.Interfaces;
using UserModule.ServiceErrors;
using ErrorType = ErrorOr.ErrorType;

namespace UserModule.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(ITokenService tokenService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AuthenticationService> logger)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<ErrorOr<UserResponse>> RegisterAsync(RegisterRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.EmailAddress);
        if (user is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        var newUser = MapToApplicationUser(request);
        var result = await _userManager.CreateAsync(newUser, request.Password);
        if (result.Succeeded)
        {
            return new UserResponse(Id: newUser.Id,
                FirstName: newUser.FirstName,
                LastName: newUser.LastName,
                EmailAddress: newUser.Email!,
                Role: newUser.Role,
                WalletBalance: newUser.WalletBalance,
                AccessToken: _tokenService.CreateUserJwt(newUser.Email!, newUser.Role, newUser.Id));
        }

        var errors = result.Errors.Select(error => Error.Validation("User." + error.Code, error.Description)).ToList();
        return errors;
    }

    public async Task<ErrorOr<UserResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.EmailAddress);
        if (user is null)
        {
            return Errors.User.NotFound;
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (signInResult.Succeeded)
        {
            _logger.LogInformation($"User {user.Id} logged in successfully.");
            return new UserResponse(Id: user.Id,
                FirstName: user.FirstName,
                LastName: user.LastName,
                EmailAddress: user.Email!,
                Role: user.Role,
                WalletBalance: user.WalletBalance,
                AccessToken: _tokenService.CreateUserJwt(user.Email!, user.Role, user.Id));
        }

        if (signInResult.IsLockedOut)
        {
            _logger.LogInformation($"User {user.Id} is locked out. End date: {user.LockoutEnd}." +
                                   $"\n\tRequest: {JsonSerializer.Serialize(request)}");
            return Errors.User.IsLockedOut;
        }

        if (signInResult.IsNotAllowed)
        {
            _logger.LogInformation($"User {user.Id} is not allowed to access the system out." +
                                   $"\n\tRequest: {JsonSerializer.Serialize(request)}");
            return Errors.User.IsNotAllowed;
        }

        _logger.LogError($"Login failed for user {user.Id}.\n\tRequest: {JsonSerializer.Serialize(request)}");
        return Errors.Auth.LoginFailed;
    }

    private static ApplicationUser MapToApplicationUser(RegisterRequest request)
    {
        return new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.EmailAddress,
            UserName = request.EmailAddress,
            Role = request.Role,
        };
    }
}