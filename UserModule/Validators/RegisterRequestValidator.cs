using FluentValidation;
using Shared.UserModels.Requests;
using UserModule.ServiceErrors;

namespace UserModule.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x)
            .NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().Length(3, 50)
            .WithErrorCode(Errors.User.InvalidFirstName.Code)
            .WithMessage(Errors.User.InvalidFirstName.Description);
        RuleFor(x => x.LastName).NotEmpty().Length(3, 50)
            .WithErrorCode(Errors.User.InvalidLastName.Code)
            .WithMessage(Errors.User.InvalidLastName.Description);
        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress()
            .WithErrorCode(Errors.User.InvalidEmailAddress.Code)
            .WithMessage(Errors.User.InvalidEmailAddress.Description);
    }
}