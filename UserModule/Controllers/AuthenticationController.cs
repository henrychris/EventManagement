using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.API;
using Shared.Extensions;
using Shared.Responses;
using Shared.UserModels.Requests;
using UserModule.Interfaces;

namespace UserModule.Controllers;

public class AuthenticationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ITokenService _tokenService;

    public AuthenticationController(IAuthenticationService authenticationService, ITokenService tokenService)
    {
        _authenticationService = authenticationService;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("tokenTest")]
    public IActionResult CreateToken(string emailAddress, string role, string userId)
    {
        var token = _tokenService.CreateUserJwt(emailAddress, role, userId);
        return Ok(new ApiResponse<string>(token, "Success", true));
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync([Required] RegisterRequest request)
    {
        var registerResult = await _authenticationService.RegisterAsync(request);
        return registerResult.Match(_ => Ok(registerResult.ToSuccessfulApiResponse()), ReturnErrorResponse);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([Required] LoginRequest request)
    {
        var loginResult = await _authenticationService.LoginAsync(request);
        return loginResult.Match(
            _ => Ok(loginResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }
}