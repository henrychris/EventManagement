using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.API;
using Shared.Extensions;
using Shared.UserModels.Requests;
using UserModule.Interfaces;

namespace UserModule.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
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