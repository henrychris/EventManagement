using ErrorOr;

namespace Shared;

public static class SharedErrors
{
    public static Error GenericError => Error.Unexpected(
        code: "Event.GenericError",
        description: "Sorry, something went wrong. Please reach out to an admin.");
}