using ErrorOr;

namespace EventModule.ServiceErrors;

public static class Errors
{
    public static class Event
    {
        public static Error NotFound => Error.NotFound(
            code: "Event.NotFound",
            description: "Event not found.");

        public static Error MissingEventName => Error.Validation(
            code: "Event.MissingEventName",
            description: "The event has no name.");

        public static Error MissingEventDescription => Error.Validation(
            code: "Event.MissingEventDescription",
            description: "The event has no description.");

        public static Error InvalidName => Error.Validation(
            code: "Event.InvalidName",
            description: $"Event name must be at least {Data.Models.Event.MinNameLength}" +
                         $" characters long and at most {Data.Models.Event.MaxNameLength} characters long.");

        public static Error InvalidDescription => Error.Validation(
            code: "Event.InvalidDescription",
            description: $"Event description must be at least {Data.Models.Event.MinDescriptionLength}" +
                         $" characters long and at most {Data.Models.Event.MaxDescriptionLength} characters long.");

        public static Error InvalidEventDate => Error.Validation(
            code: "Event.InvalidEventDate",
            description: "The event can't start in the past.");

        public static Error InvalidEventStartTime => Error.Validation(
            code: "Event.InvalidEventStartTime",
            description: "The event can't start before the registered date.");

        public static Error InvalidEventEndTime => Error.Validation(
            code: "Event.InvalidEventEndTime",
            description: "The event can't end before its date or start time.");

        public static Error InvalidTicketPrice => Error.Validation(
            code: "Event.InvalidTicketPrice",
            description: "The ticket price must be greater than, or equal to zero.");

        // todo: add invalid capacity error
    }
}