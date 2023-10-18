using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Responses;
using UserModule.Interfaces;

namespace UserModule.Controllers;

public class AuthenticationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [HttpPost("tokenTest")]
    public IActionResult CreateToken(string emailAddress, string role, string userId)
    {
        var token = _authenticationService.CreateUserJwt(emailAddress, role, userId);
        return Ok(new ApiResponse<string>(token, "Success", true));
    }
}