using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Shared.API;
using Shared.Extensions;
using Shared.UserModels.Responses;
using UserModule.Interfaces;

namespace UserModule.Controllers;

public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var getUserResult = await _userService.GetUser(id.ToString());
        
        return getUserResult.Match(
            _ => Ok(getUserResult.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }
}