using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Responses;

namespace Shared;

[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    protected static IActionResult ReturnErrorResponse(List<Error> errors)
    {
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            return CreateValidationError(errors);
        }

        if (errors.Any(e => e.Type == ErrorType.Unexpected))
        {
            return new ObjectResult(new ApiErrorResponse<List<Error>>(errors, "Something went wrong."))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return new ObjectResult(new ApiErrorResponse<List<Error>>(errors, firstError.Description))
        {
            StatusCode = statusCode
        };
    }

    private static IActionResult CreateValidationError(List<Error> errors)
    {
        var problemDetails = new ApiErrorResponse<object>(
            errors.ToDictionary(e => e.Code,
                e => new[]
                {
                    e.Description
                }),
            message: "One or more validation errors occured.");

        return new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}