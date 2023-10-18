using ErrorOr;

namespace UserModule.ServiceErrors;

public static partial class Errors
{
    public static class Auth
    {
        public static Error LoginFailed => Error.Unauthorized(
            code: "Auth.LoginFailed",
            description: "Login failed. Please try again later.");
    }
}