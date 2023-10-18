using ErrorOr;
using FluentValidation.Results;

namespace UserModule.Extensions;

public static class ValidationResultExtensions
{
    public static List<Error> ToErrorList(this ValidationResult validationResult)
    {
        return validationResult.Errors.Select(x => Error.Validation(x.ErrorCode, x.ErrorMessage))
            .ToList();
    }
}