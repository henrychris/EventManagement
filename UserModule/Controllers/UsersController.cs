﻿using Microsoft.AspNetCore.Mvc;
using Shared.API;
using Shared.Extensions;
using UserModule.Interfaces;

namespace UserModule.Controllers;

public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
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
    
    // todo: get user events with message broker
    // /users/{userid}/events
}