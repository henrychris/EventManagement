using ErrorOr;

namespace EventModule.ServiceErrors;

public static class Errors
{
    public static class Event
    {
        public static Error NotFound => Error.NotFound(
            code: "Event.NotFound",
            description: "Event not found");
    }
}