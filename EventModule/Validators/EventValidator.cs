using EventModule.Data.Models;
using EventModule.ServiceErrors;
using FluentValidation;

namespace EventModule.Validators;

public class EventValidator : AbstractValidator<Event>
{
    public EventValidator()
    {
        RuleFor(request => request)
            .NotEmpty();
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(Errors.Event.MissingEventName.Description)
            .WithErrorCode(Errors.Event.MissingEventName.Code)
            .Length(Event.MinNameLength, Event.MaxNameLength)
            .WithMessage(Errors.Event.InvalidName.Description)
            .WithErrorCode(Errors.Event.InvalidName.Code);

        RuleFor(request => request.Description)
            .NotEmpty()
            .WithMessage(Errors.Event.MissingEventDescription.Description)
            .WithErrorCode(Errors.Event.MissingEventDescription.Code)
            .Length(Event.MinDescriptionLength, Event.MaxDescriptionLength)
            .WithMessage(Errors.Event.InvalidDescription.Description)
            .WithErrorCode(Errors.Event.InvalidDescription.Code);

        RuleFor(request => request.Date)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage(Errors.Event.InvalidEventDate.Description)
            .WithErrorCode(Errors.Event.InvalidEventDate.Code);

        RuleFor(request => request.StartTime)
            .GreaterThanOrEqualTo(request => request.Date)
            .WithMessage(Errors.Event.InvalidEventStartTime.Description)
            .WithErrorCode(Errors.Event.InvalidEventStartTime.Code);

        RuleFor(request => request.EndTime)
            .GreaterThanOrEqualTo(request => request.Date)
            .WithMessage(Errors.Event.InvalidEventEndTime.Description)
            .WithErrorCode(Errors.Event.InvalidEventEndTime.Code)
            .GreaterThanOrEqualTo(request => request.StartTime)
            .WithMessage(Errors.Event.InvalidEventEndTime.Description)
            .WithErrorCode(Errors.Event.InvalidEventEndTime.Code);

        RuleFor(request => request.Price)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(Errors.Event.InvalidTicketPrice.Code)
            .WithMessage(Errors.Event.InvalidTicketPrice.Description);

        RuleFor(request => request.TicketsAvailable)
            .GreaterThanOrEqualTo(Event.MinEventAttendance)
            .WithErrorCode(Errors.Event.InvalidCapacity.Code)
            .WithMessage(Errors.Event.InvalidCapacity.Description);

        RuleFor(request => request.TicketsAvailable)
            .LessThanOrEqualTo(Event.MaxEventAttendance)
            .WithErrorCode(Errors.Event.ExceedsMaximumCapacity.Code)
            .WithMessage(Errors.Event.ExceedsMaximumCapacity.Description);
    }
}