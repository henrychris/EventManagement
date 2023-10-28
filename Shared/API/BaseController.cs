using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Filters;
using Shared.Responses;

namespace Shared.API;

[Authorize]
[ApiController]
[TypeFilter(typeof(CustomValidationFilter))]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Returns an IActionResult object based on the list of errors passed as parameter.
    /// </summary>
    /// <param name="errors">List of errors to be handled.</param>
    /// <returns>An IActionResult object based on the type of errors.</returns>
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

        var problemDetails = new ApiErrorResponse<object>(
            errors.ToDictionary(e => e.Code,
                e => new[]
                {
                    e.Description
                }),
            message: firstError.Description);

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }

    /// <summary>
    /// Creates a validation error response with the given list of errors.
    /// </summary>
    /// <param name="errors">The list of errors to include in the response.</param>
    /// <returns>An IActionResult representing the validation error response.</returns>
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