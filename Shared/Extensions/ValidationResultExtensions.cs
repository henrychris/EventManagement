using ErrorOr;
using FluentValidation.Results;

namespace Shared.Extensions;

public static class ValidationResultExtensions
{
    /// <summary>
    /// Extension method to convert a ValidationResult object to a list of Error objects.
    /// </summary>
    /// <param name="validationResult">The ValidationResult object to convert.</param>
    /// <returns>A list of Error objects.</returns>
    public static List<Error> ToErrorList(this ValidationResult validationResult)
    {
        return validationResult.Errors.Select(x => Error.Validation(x.ErrorCode, x.ErrorMessage))
            .ToList();
    }
}