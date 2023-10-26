using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Responses;

namespace Shared.Filters;

public class CustomValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Check if the model state is invalid
        if (!context.ModelState.IsValid)
        {
            // Create a custom response
            var customErrorResponse = new ApiErrorResponse<object>(
                context.ModelState
                    .Where(x => x.Value is { Errors.Count: > 0 })
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                    ),
                "Validation failed"
            );

            // Set the custom response as the result
            context.Result = new ObjectResult(customErrorResponse)
            {
                StatusCode = 400,
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No action needed after the action is executed
    }
}